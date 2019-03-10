using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using PickUpSports.DAL;
using PickUpSports.Interface;
using PickUpSports.Models.DatabaseModels;
using PickUpSports.Models.GoogleApiModels;
using PickUpSports.Models.ViewModel;

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

        public ActionResult Index(string sortBy)
        {
            // Only want to update Venues database once a week
            Venue mostRecentUpdate = _context.Venues.OrderByDescending(v => v.DateUpdated).FirstOrDefault();

            // If there is no update or if most recent update was more than week ago
            // search places and update Venues database
            if (mostRecentUpdate == null || mostRecentUpdate.DateUpdated < DateTime.Now.AddDays(-7))
            {
                // Get list of places 
                PlaceSearchResponse places = _placesApi.GetPlaces().Result;
                
                foreach (var place in places.Results)
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

                UpdatePlaceDetails();
            }

            List<VenueViewModel> model = new List<VenueViewModel>();
            List<Venue> venues = _context.Venues.ToList();

            foreach (var venue in venues)
            {
                // get the rating 
                List<Review> reviews = _context.Reviews.Where(r => r.VenueId == venue.VenueId).ToList();
                decimal avgRating = (decimal)reviews.Average(r => r.Rating);

                model.Add(new VenueViewModel
                {
                    Address1 = venue.Address1,
                    Address2 = venue.Address2,
                    City = venue.City,
                    Name = venue.Name,
                    Phone = venue.Phone,
                    State = venue.State,
                    VenueId = venue.VenueId,
                    ZipCode = venue.ZipCode,
                    // add rating to the model so can sort by rating
                    AverageRating = avgRating
                });
                
            }

            //implement sorting by rate fuction
            ViewBag.RateSort = string.IsNullOrEmpty(sortBy) ? "RatingDesc" : "";
            switch (sortBy)
            {
                case "RatingDesc":
                    model=model.OrderByDescending(x=>x.AverageRating).ToList();
                    break;
                default:
                    model=model.OrderBy(x => x.VenueId).ToList();
                    break;
            }
            return View(model);
        }

<<<<<<< HEAD
=======
        public ActionResult Map()
        {
            return View();
        }

       
       
>>>>>>> c381d77a0a98f95f60a8a92897e31d672f7a0b5c

        /**
         * Get venue, hours, and review data for single Venue and return to view
         */
        public ActionResult Details(int id)
        {
            // Model to be sent to view
            VenueViewModel model = new VenueViewModel();

            // Map venue details
            Venue venue = _context.Venues.Find(id);
            model.Address1 = venue.Address1;
            model.Address2 = venue.Address2;
            model.City = venue.City;
            model.Name = venue.Name;
            model.Phone = venue.Phone;
            model.State = venue.State;
            model.VenueId = venue.VenueId;
            model.ZipCode = venue.ZipCode;

            // Map business hours
            List<BusinessHours> businessHours = _context.BusinessHours.Where(b => b.VenueId == id).ToList();
            model.BusinessHours = new List<BusinessHoursViewModel>();

            foreach (var businessHour in businessHours)
            {
                var closeDateTime = new DateTime() + businessHour.CloseTime;
                var openDateTime = new DateTime() + businessHour.OpenTime;

                model.BusinessHours.Add(new BusinessHoursViewModel
                {
                    DayOfWeek = Enum.GetName(typeof(DayOfWeek), businessHour.DayOfWeek),
                    CloseTime = closeDateTime.ToShortTimeString(),
                    OpenTime = openDateTime.ToShortTimeString()
                });
            }

            // Map reviews 
            List<Review> reviews = _context.Reviews.Where(r => r.VenueId == id).ToList();
            model.Reviews = new List<ReviewViewModel>();

            decimal avgRating = (decimal) reviews.Average(r => r.Rating);
            model.AverageRating = Math.Round(avgRating, 1);

            List<ReviewViewModel> tempList = new List<ReviewViewModel>();
            foreach (var review in reviews)
            {
                ReviewViewModel reviewModel = new ReviewViewModel
                {
                    Rating = review.Rating,
                    Timestamp = review.Timestamp,
                    Comments = review.Comments
                };

                // If review is not a Google review then there is no GoogleAuthor, 
                // find author in Contact table
                if (review.IsGoogleReview) reviewModel.Author = review.GoogleAuthor;
                else reviewModel.Author = _context.Contacts.Find(review.ContactId).Username;

                tempList.Add(reviewModel);
            }

            // Order reviews newest to oldest
            model.Reviews = tempList.OrderByDescending(r => r.Timestamp).ToList();
            return View(model);
        }

        /**
         * Method that checks which Venues have missing details and updates those
         * details with details from Google Place API. Uses API response to update Venue
         * details and BusinessHours table data 
         */
        private void UpdatePlaceDetails()
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
                        hours.OpenTime = DateTime.Parse(openTime).TimeOfDay;

                        string closeTime = period.Close?.Time.Insert(2, ":");
                        hours.CloseTime = DateTime.Parse(closeTime).TimeOfDay;

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

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _context.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}