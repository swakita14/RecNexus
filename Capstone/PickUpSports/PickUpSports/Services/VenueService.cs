﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using PickUpSports.DAL;
using PickUpSports.Interface;
using PickUpSports.Models.DatabaseModels;
using PickUpSports.Models.GoogleApiModels;
using System.Linq;

namespace PickUpSports.Services
{
    public class VenueService : IVenueService
    {
        private readonly IPlacesApiClient _placesApi;
        private readonly PickUpContext _context;

        public VenueService(IPlacesApiClient placesApi, PickUpContext context)
        {
            _placesApi = placesApi;
            _context = context;
        }

        /**
         * Add or update new venues in database
         */
        public void UpdateVenues()
        {
            // Only want to update Venues database once a week
            Venue mostRecentUpdate = _context.Venues.OrderByDescending(v => v.DateUpdated).FirstOrDefault();

            // If there is no update or if most recent update was more than two weeks ago
            // search places and update Venues database
            if (mostRecentUpdate == null || mostRecentUpdate.DateUpdated < DateTime.Now.AddDays(-14))
            {
                // Get list of places 
                List<PlaceSearchResult> places = _placesApi.GetVenues().Result;

                foreach (var place in places)
                {
                    // Check if we already have this venue in database
                    Venue existingVenue = _context.Venues.FirstOrDefault(v => v.GooglePlaceId == place.PlaceId);

                    // If not in database, add it
                    if (existingVenue == null)
                    {
                        Venue venue = new Venue
                        {
                            GooglePlaceId = place.PlaceId,
                            Name = place.Name,
                            DateUpdated = DateTime.Now
                        };

                        _context.Venues.Add(venue);
                        _context.SaveChanges();
                    }
                }

                // Update most recent update
                mostRecentUpdate.DateUpdated = DateTime.Now;
                _context.SaveChanges();

                UpdateVenueDetails();
            }

        }

        /**
         * Method to  calculate distance via Haversine formula.
         */
        public double CalculateVenueDistance(double lat1, double long1, double lat2, double long2)
        {
            double dDistance = Double.MinValue;
            double dLat1InRad = lat1 * (Math.PI / 180.0);
            double dLong1InRad = long1 * (Math.PI / 180);
            double dLat2InRad = lat2 * (Math.PI / 180.0);
            double dLong2InRad = long2 * (Math.PI / 180.0);

            double dLongitude = dLong2InRad - dLong1InRad;
            double dLatitude = dLat2InRad - dLat1InRad;

            // Intermediate result a.
            double a = Math.Pow(Math.Sin(dLatitude / 2.0), 2.0) +
                       Math.Cos(dLat1InRad) * Math.Cos(dLat2InRad) *
                       Math.Pow(Math.Sin(dLongitude / 2.0), 2.0);
            //Intermediate result c 
            double c = 2.0 * Math.Asin(Math.Sqrt(a));

            //Distance (using approximate radius of earth in miles)
            const Double kEarthRadiusMiles = 3958.8;
            dDistance = kEarthRadiusMiles * c;
            return dDistance;
        }

        /**
         * Add or update venue details in database
         */
        private void UpdateVenueDetails()
        {
            // Get all venues 
            List<Venue> venues = _context.Venues.ToList();

            // Get only venues that don't have details
            List<Venue> venuesWithoutDetails = venues.Where(v => v.Address1 == null).ToList();

            foreach (var venue in venuesWithoutDetails)
            {
                // Get Place details from Google API using GooglePlaceId
                PlaceDetailsResponse venueDetails = _placesApi.GetPlaceDetailsById(venue.GooglePlaceId).Result;

                // Map response data to database model properties
                venue.Phone = venueDetails.Result.FormattedPhoneNumber;
                string streetAddress = null;
                foreach (var addressComponent in venueDetails.Result.AddressComponents)
                {
                    // Map Address from response to database model
                    var type = addressComponent.Types.FirstOrDefault();
                    switch (type)
                    {
                        case "street_number":
                            streetAddress += addressComponent.ShortName + " ";
                            break;
                        case "route":
                            streetAddress += addressComponent.ShortName;
                            break;
                        case "locality":
                            venue.City = addressComponent.ShortName;
                            break;
                        case "administrative_area_level_1":
                            venue.State = addressComponent.ShortName;
                            break;
                        case "postal_code":
                            venue.ZipCode = addressComponent.ShortName;
                            break;
                        default:
                            continue;
                    }

                    venue.Address1 = streetAddress;
                }

                // Update Venue entity
                _context.Entry(venue).State = EntityState.Modified;
                _context.SaveChanges();

                // Map OpeningHours API response to BusinessHours entity
                if (venueDetails.Result.OpeningHours != null)
                {
                    // Initialize new BusinessHours entity using VenueID as foreign key
                    BusinessHours hours = new BusinessHours { VenueId = venue.VenueId };

                    foreach (var period in venueDetails.Result.OpeningHours.Periods)
                    {
                        hours.DayOfWeek = period.Open.Day;

                        string openTime = period.Open?.Time.Insert(2, ":");
                        if (!string.IsNullOrEmpty(openTime)) hours.OpenTime = DateTime.Parse(openTime).TimeOfDay;

                        string closeTime = period.Close?.Time.Insert(2, ":");
                        if (!string.IsNullOrEmpty(closeTime)) hours.CloseTime = DateTime.Parse(closeTime).TimeOfDay;

                        // Add BusinessHours entity
                        _context.BusinessHours.Add(hours);
                        _context.SaveChanges();
                    }
                }

                // Map Review API response to Review database entity
                if (venueDetails.Result.Reviews != null)
                {
                    foreach (var review in venueDetails.Result.Reviews)
                    {
                        DateTime timestamp = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);

                        Review reviewEntity = new Review
                        {
                            IsGoogleReview = true,
                            GoogleAuthor = review.AuthorName,
                            VenueId = venue.VenueId,
                            Comments = review.Text,
                            Rating = review.Rating,

                            Timestamp = timestamp.AddSeconds(review.Time).ToLocalTime()
                        };

                        _context.Reviews.Add(reviewEntity);
                    }

                    _context.SaveChanges();
                }

                // Map Location API response to Location database entity
                if (venueDetails.Result.Geometry != null)
                {
                    Location locationEntity = new Location
                    {
                        Latitude =
                            venueDetails.Result.Geometry.GeometryLocation.Latitude.ToString(CultureInfo.InvariantCulture),
                        Longitude =
                            venueDetails.Result.Geometry.GeometryLocation.Longitude.ToString(CultureInfo.InvariantCulture),
                        VenueId = venue.VenueId
                    };

                    // Add Location entity
                    _context.Locations.Add(locationEntity);
                    _context.SaveChanges();
                }
            }
        }
    }
}