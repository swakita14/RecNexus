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
        private readonly PickUpContext _context;
        private readonly IVenueService _venueService;

        public VenueController(PickUpContext context, IVenueService venueService)
        {
            _context = context;
            _venueService = venueService;
        }
        
        public ActionResult Index(string sortBy, string curLat, string curLong, string time, string day)
        {
            _venueService.UpdateVenues();
            _venueService.UpdateVenueDetails();

            // Create view model for list of venues
            List<VenueViewModel> model = new List<VenueViewModel>();
            List<Venue> venues = _context.Venues.ToList();

            foreach (var venue in venues)
            {
                //get location of venue
                Location location = _context.Locations
                    .FirstOrDefault(l => l.VenueId == venue.VenueId);

                //converted coordinates from strings to doubles
                double userLat = Convert.ToDouble(curLat);
                double userLong = Convert.ToDouble(curLong);
                double venLat = Convert.ToDouble(location.Latitude);
                double venLong = Convert.ToDouble(location.Longitude);
                
                //Calculate the distance from user to venue. Method at the bottom
                double distance = _venueService.CalculateVenueDistance(userLat, userLong, venLat, venLong);


                // get the rating 
                List<Review> reviews = _context.Reviews.Where(r => r.VenueId == venue.VenueId).ToList();

                decimal? avgRating;
                if (reviews.Count > 0)
                {
                    avgRating = (decimal) reviews.Average(r => r.Rating);
                }
                else avgRating = null;


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
                    AverageRating = avgRating,
                    LatitudeCoord = location.Latitude,
                    LongitudeCoord = location.Longitude,
                    Distance = distance
                });

            }

            //get all the buiness hours for all the venues
            List<BusinessHours> hours = _context.BusinessHours.ToList();

            //lets wait till time is NOT null
            if (time != null)
            {
                //convert user input string to a TimeSpan
                TimeSpan hs = TimeSpan.Parse(time);

                var dayOfWeek = Enum.Parse(typeof(DayOfWeek), day);
                //First filter out using the days
                List<BusinessHours> day_available = hours.Where(x => x.DayOfWeek == (int) dayOfWeek).ToList();

                //Then use that list and filter the closed times
                List<BusinessHours> closed_from = day_available.Where(x => x.CloseTime >= hs).ToList();

                //Finally filter the list above and you have the remaning few which maches the days and the closing time
                List<BusinessHours> open_from = closed_from.Where(x => x.OpenTime <= hs).ToList();

                //If there are some in the list that matches the user input - else it didnt match
                if (open_from.Count > 0)
                {
                    // Clear current list so can add only venues that will be open during chosen time
                    model.Clear();

                    foreach (var v in open_from)
                    {
                        Venue venue = _context.Venues.Find(v.VenueId);
                        
                        // Add venue to model
                        model.Add(new VenueViewModel
                        {
                            Address1 = venue.Address1,
                            Address2 = venue.Address2,
                            City = venue.City,
                            Name = venue.Name,
                            Phone = venue.Phone,
                            State = venue.State,
                            VenueId = venue.VenueId,
                            ZipCode = venue.ZipCode
                        });
                    }

                    return View(model);

                }
                else
                {
                    //Error Message shows 
                    ViewBag.Message = "No Match Available";
                }
            }


            ViewBag.DistanceSort = string.IsNullOrEmpty(sortBy) ? "Distance" : "";
            //implement sorting by rate fuction
            ViewBag.RateSort = string.IsNullOrEmpty(sortBy) ? "RatingDesc" : "";

            switch (sortBy)
            {
                case "RatingDesc":
                    model=model.OrderByDescending(x=>x.AverageRating).ToList();
                    break;
                case "Distance":
                    model = model.OrderBy(x => x.Distance).ToList();
                    break;
                default:
                    model=model.OrderBy(x => x.VenueId).ToList();
                    break;
            }
            return View(model);
        }


        public ActionResult Map()
        {
            return View();
        }

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

            decimal? avgRating;
            if (reviews.Count > 0)
            {
                avgRating = (decimal) reviews.Average(r => r.Rating);
                model.AverageRating = Math.Round((decimal) avgRating, 1);
            }
            else
            {
                avgRating = null;
                model.AverageRating = null;
            }

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
                else
                {
                    Contact user = _context.Contacts.Find(review.ContactId);
                    if (user == null) reviewModel.Author = null;
                    else reviewModel.Author = user.Username;
                }

                tempList.Add(reviewModel);
            }

            // Order reviews newest to oldest
            model.Reviews = tempList.OrderByDescending(r => r.Timestamp).ToList();
            return View(model);
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