using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using PickUpSports.DAL;
using PickUpSports.Interface;
using PickUpSports.Models.DatabaseModels;
using PickUpSports.Models.Enums;
using PickUpSports.Models.Extensions;
using PickUpSports.Models.ViewModel;
using PickUpSports.Models.ViewModel.GameController;
using PickUpSports.Models.ViewModel.VenueController;

namespace PickUpSports.Controllers
{
    public class VenueController : Controller
    {
        private readonly IVenueService _venueService;
        private readonly IContactService _contactService;
        private readonly IGameService _gameService;

        public VenueController(IVenueService venueService, 
            IContactService contactService, 
            IGameService gameService)
        {
            _venueService = venueService;
            _contactService = contactService;
            _gameService = gameService;
        }
        
        /*
         * Show a list of venues
         */
        public ActionResult Index()
        {
            _venueService.UpdateVenues();

            // Create view model for list of venues
            SearchVenueViewModel model = new SearchVenueViewModel();
            model.Venues = new List<VenueViewModel>();
            List<Venue> venues = _venueService.GetAllVenues();

            foreach (var venue in venues)
            {
                // get location of venue
                Location location = _venueService.GetVenueLocation(venue.VenueId);
                
                // get the rating 
                List<Review> reviews = _venueService.GetVenueReviews(venue.VenueId);

                decimal? avgRating;
                if (reviews.Count > 0)
                {
                    avgRating = (decimal) reviews.Average(r => r.Rating);
                }
                else avgRating = null;


                model.Venues.Add(new VenueViewModel
                {
                    Address1 = venue.Address1,
                    Address2 = venue.Address2,
                    City = venue.City,
                    Name = venue.Name,
                    Phone = venue.Phone,
                    State = venue.State,
                    VenueId = venue.VenueId,
                    ZipCode = venue.ZipCode,
                    AverageRating = avgRating,
                    LatitudeCoord = location.Latitude,
                    LongitudeCoord = location.Longitude,
                });
            }

            return View(model);
        }

        /*
         * Venue search and filter results
         */
        [HttpPost]
        public ActionResult Filter(SearchVenueViewModel model)
        {
            model.Venues = (List<VenueViewModel>) TempData["Venues"];

            // Create new model to send to view
            SearchVenueViewModel viewModel = new SearchVenueViewModel();
            viewModel.Venues = new List<VenueViewModel>();

            if (model.Venues.Count == 0)
            {
                ViewBag.Error = "No results to filter. Please click \"Reset all filters\"";
                return View("Index", viewModel);
            }

            // User entered a search string
            if (!string.IsNullOrEmpty(model.Search))
            {
                // Search venue names and cities
                viewModel.Venues.AddRange(model.Venues.Where(x => x.Name.CaseInsensitiveContains(model.Search)).ToList());
                viewModel.Venues.AddRange(model.Venues.Where(x => x.City.CaseInsensitiveContains(model.Search)));
                
                // Get first venue on list and assign coordinates to map center
                var firstVenue = viewModel.Venues.FirstOrDefault();
                if (firstVenue != null)
                {
                    viewModel.CurrentLatitude = firstVenue.LatitudeCoord;
                    viewModel.CurrentLongitude = firstVenue.LongitudeCoord;
                }
            }

            if (!string.IsNullOrEmpty(model.Filter))
            {
                // User chose to sort by rating
                if (model.Filter.Equals("Rating"))
                {
                    viewModel.Venues = model.Venues.OrderByDescending(x => x.AverageRating).ToList();

                    if (viewModel.Venues.Count == 0)
                    {
                        ViewBag.Error = "No search results matching given filters";
                    }

                    // Get first venue on list and assign coordinates to map center
                    var firstVenue = viewModel.Venues.FirstOrDefault();
                    if (firstVenue != null)
                    {
                        viewModel.CurrentLatitude = firstVenue.LatitudeCoord;
                        viewModel.CurrentLongitude = firstVenue.LongitudeCoord;
                    }
                }
                // User chose to filter by time
                if (model.Filter.Equals("Time"))
                {
                    //get all the buiness hours for all the venues
                    List<BusinessHours> hours = _venueService.GetAllBusinessHours();

                    //lets wait till time is NOT null
                    if (model.Time != null)
                    {
                        //convert user input string to a TimeSpan
                        TimeSpan hs = TimeSpan.Parse(model.Time);

                        var dayOfWeek = Enum.Parse(typeof(DayOfWeek), model.Day);
                        //First filter out using the days
                        List<BusinessHours> day_available = hours.Where(x => x.DayOfWeek == (int) dayOfWeek).ToList();

                        //Then use that list and filter the closed times
                        List<BusinessHours> closed_from = day_available.Where(x => x.CloseTime >= hs).ToList();

                        //Finally filter the list above and you have the remaning few which maches the days and the closing time
                        List<BusinessHours> open_from = closed_from.Where(x => x.OpenTime <= hs).ToList();

                        //If there are some in the list that matches the user input - else it didnt match
                        if (open_from.Count > 0)
                        {
                            foreach (var businessHour in open_from)
                            {
                                // Only keep venues that are in open_from
                                var venue = model.Venues.FirstOrDefault(x => x.VenueId == businessHour.VenueId);
                                if (venue != null && !viewModel.Venues.Contains(venue)) viewModel.Venues.Add(venue);
                            }
                        }
                    }

                    // Get first venue on list and assign coordinates to map center
                    var firstVenue = viewModel.Venues.FirstOrDefault();
                    if (firstVenue != null)
                    {
                        viewModel.CurrentLatitude = firstVenue.LatitudeCoord;
                        viewModel.CurrentLongitude = firstVenue.LongitudeCoord;
                    }
                }

                // User chose to filter by distance
                if (model.Filter.Equals("Distance"))
                {
                    foreach (var venue in model.Venues)
                    {
                        //get location of venue
                        Location location = _venueService.GetVenueLocation(venue.VenueId);

                        //converted coordinates from strings to doubles
                        double userLat = Convert.ToDouble(model.CurrentLatitude);
                        double userLong = Convert.ToDouble(model.CurrentLongitude);
                        double venLat = Convert.ToDouble(location.Latitude);
                        double venLong = Convert.ToDouble(location.Longitude);

                        //Calculate the distance from user to venue. Method at the bottom
                        venue.Distance = _venueService.CalculateVenueDistance(userLat, userLong, venLat, venLong);
                        viewModel.Venues.Add(venue);
                    }

                    viewModel.Venues = viewModel.Venues.OrderBy(x => x.Distance).ToList();

                    // Get closest venue to user and assign coordinates to map center
                    var closest = viewModel.Venues.First();
                    viewModel.CurrentLatitude = closest.LatitudeCoord;
                    viewModel.CurrentLongitude = closest.LongitudeCoord;

                }
            }
           
            if (viewModel.Venues.Count == 0)
            {
                ViewBag.Error = "No search results matching given filters";
            }

            // Preserve search/filter values
            viewModel.Day = model.Day;
            viewModel.Time = model.Time;
            viewModel.Search = model.Search;

            if (string.IsNullOrEmpty(viewModel.CurrentLatitude))
            {
                viewModel.CurrentLatitude = model.CurrentLatitude;
                viewModel.CurrentLongitude = model.CurrentLongitude;
            }

            model.Venues.Clear();
            return View("Index", viewModel);
        }

        /*
         * Get venue, hours, and review data for single Venue and return to view
         */
        public ActionResult Details(int id)
        {
            // Model to be sent to view
            VenueViewModel model = new VenueViewModel();

            // Map venue details
            Venue venue = _venueService.GetVenueById(id);
            model.Address1 = venue.Address1;
            model.Address2 = venue.Address2;
            model.City = venue.City;
            model.Name = venue.Name;
            model.Phone = venue.Phone;
            model.State = venue.State;
            model.VenueId = venue.VenueId;
            model.ZipCode = venue.ZipCode;

            // Map business hours
            List<BusinessHours> businessHours = _venueService.GetVenueBusinessHours(id);
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
            List<Review> reviews = _venueService.GetVenueReviews(id);
            model.Reviews = new List<ReviewViewModel>();

            decimal? avgRating;
            if (reviews.Count > 0)
            {
                avgRating = (decimal) reviews.Average(r => r.Rating);
                model.AverageRating = Math.Round((decimal) avgRating, 1);
            }
            else model.AverageRating = null;         

            return View(model);
        }

        public PartialViewResult GetReviews(int venueId)
        {
            var reviews = _venueService.GetVenueReviews(venueId);
            var model = new PartialReviewViewModel{VenueId = venueId};

            if (reviews.Count > 0)
            {
                var averageRating = (decimal)reviews.Average(r => r.Rating);
                model.AverageReviewRating = Math.Round((decimal)averageRating, 1);
            }
            else model.AverageReviewRating = null;

            List<ReviewViewModel> tempList = new List<ReviewViewModel>();
            foreach (var review in reviews)
            {
                ReviewViewModel reviewModel = new ReviewViewModel
                {
                    Rating = review.Rating,
                    Timestamp = review.Timestamp,
                    Comments = review.Comments,
                    ReviewId = review.ReviewId
                };

                // If review is not a Google review then there is no GoogleAuthor, 
                // find author in Contact table
                if (review.IsGoogleReview) reviewModel.Author = review.GoogleAuthor;
                else
                {
                    Contact user = _contactService.GetContactById(review.ContactId);
                    if (user == null) reviewModel.Author = null;
                    else
                    {
                        reviewModel.Author = user.Username;

                        // Check if the logged in user wrote this review
                        string loggedInUserEmail = User.Identity.GetUserName();

                        if (user.Email == loggedInUserEmail)
                        {
                            reviewModel.ReviewBelongsToUser = true;
                        }
                        else reviewModel.ReviewBelongsToUser = false;
                    }
                }

                tempList.Add(reviewModel);
            }

            // Order reviews newest to oldest
            model.Reviews = tempList.OrderByDescending(r => r.Timestamp).ToList();
            return PartialView("_VenueReviews", model);
        }

        public PartialViewResult GetVenueGames(int venueId)
        {
            var model = new List<GameListViewModel>();
            var games = _gameService.GetCurrentGamesByVenueId(venueId).Where(x => x.GameStatusId == (int)GameStatusEnum.Open);

            foreach (var game in games)
            {
                GameListViewModel gameToAdd = new GameListViewModel
                {
                    GameId = game.GameId,
                    Sport = _gameService.GetSportNameById(game.SportId),
                    StartDate = game.StartTime,
                    EndDate = game.EndTime
                };

                if (game.ContactId != null)
                {
                    gameToAdd.ContactId = game.ContactId;
                    gameToAdd.ContactName = _contactService.GetContactById(game.ContactId).Username;
                }

                model.Add(gameToAdd);
            }

            model = new List<GameListViewModel>(model.OrderBy(x => x.StartDate));
            return PartialView("_VenueGames", model);
        }
    }
}