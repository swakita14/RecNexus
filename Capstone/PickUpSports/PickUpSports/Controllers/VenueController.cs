using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using PickUpSports.Data.GoogleAPI.Interfaces;
using PickUpSports.Data.GoogleAPI.Models;
using PickUpSports.DAL;
using PickUpSports.Models;

namespace PickUpSports.Controllers
{
    public class VenueController : Controller
    {
        private readonly IPlacesApiClient _placesApi;
        private readonly PickUpContext _context;

        public VenueController(IPlacesApiClient placesApi, PickUpContext context)
        {
            _placesApi = placesApi;
            _context = context;
        }

        public async Task<ActionResult> Index()
        {
            // Only want to update Venues database once a week
            var mostRecentUpdate = await _context.Venues.OrderByDescending(v => v.DateUpdated).FirstOrDefaultAsync();

            // If there is no update or if most recent update was more than week ago
            // search places and update Venues database
            if (mostRecentUpdate == null || mostRecentUpdate.DateUpdated < DateTime.Now.AddDays(-7))
            {
                // Get list of places 
                PlaceSearchResponse places = await _placesApi.GetPlaces();
                
                foreach (var place in places.Results)
                {
                    // Check if we already have this venue in database
                    Venue existingVenue = await _context.Venues.FirstOrDefaultAsync(v => v.GooglePlaceId == place.PlaceId);

                    // If not in database, add it
                    if (existingVenue == null)
                    {
                        Venue venue = new Venue { GooglePlaceId = place.PlaceId, Name = place.Name, DateUpdated = DateTime.Now};
                        _context.Venues.Add(venue);
                        await _context.SaveChangesAsync();
                    }
                }
            }

            await UpdatePlaceDetails();

            return View();
        }

        /**
         * Method that checks which Venues have missing details and updates those
         * details with details from Google Place API. Uses API response to update Venue
         * details and BusinessHours table data 
         */
        private async Task UpdatePlaceDetails()
        {
            // Get all venues 
            List<Venue> venues = await _context.Venues.ToListAsync();

            // Get only venues that don't have details
            List<Venue> venuesWithoutDetails = venues.Where(v => v.Address1 == null).ToList();

            foreach (var venue in venuesWithoutDetails)
            {
                // Get Place details from Google API using GooglePlaceId
                var venueDetails = await _placesApi.GetPlaceDetailsById(venue.GooglePlaceId);

                // Map response data to database model properties
                venue.Phone = venueDetails.Result.FormattedPhoneNumber;
                string streetAddress = null;
                foreach (var addressComponent in venueDetails.Result.AddressComponents)
                {
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
                await _context.SaveChangesAsync();
                
                // Map OpeningHours API response to BusinessHours entity
                if (venueDetails.Result.OpeningHours != null)
                {
                    // Initialize new BusinessHours entity using VenueID as foreign key
                    BusinessHours hours = new BusinessHours { VenueId = venue.VenueId };

                    foreach (var period in venueDetails.Result.OpeningHours.Periods)
                    {
                        hours.DayOfWeek = period.Open.Day;

                        DateTime.TryParseExact(period.Open?.Time, "HHmm", CultureInfo.InvariantCulture,
                            DateTimeStyles.None, out var openDateTime);
                        hours.OpenTime = openDateTime.TimeOfDay;

                        DateTime.TryParseExact(period.Close?.Time, "HHmm", CultureInfo.InvariantCulture,
                            DateTimeStyles.None, out var closeDateTime);
                        hours.CloseTime = closeDateTime.TimeOfDay;

                        // Add BusinessHours entity
                        _context.BusinessHours.Add(hours);
                        await _context.SaveChangesAsync();
                    }
                }
            }
        }
    }
}