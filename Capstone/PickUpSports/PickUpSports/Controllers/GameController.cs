using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using PickUpSports.DAL;
using PickUpSports.Models.DatabaseModels;
using PickUpSports.Models.Enums;
using PickUpSports.Models.ViewModel;
using PickUpSports.Models.ViewModel.GameController;
using DayOfWeek = System.DayOfWeek;

namespace PickUpSports.Controllers
{
    public class GameController : Controller
    {
        private readonly PickUpContext _context;

        public GameController(PickUpContext context)
        {
            _context = context;
        }

        /**
         * Routes user to page that contains Create Game form
         */
        public ActionResult CreateGame()
        {
            ViewBag.GameCreated = false;
            // Confirm user is logged in (visitors can't create game)
            string email = User.Identity.GetUserName();
            Contact contact = _context.Contacts.FirstOrDefault(c => c.Email == email);

            if (contact == null)
            {
                ModelState.AddModelError("NoContact", "Please login or register to start a game.");
                PopulateDropdownValues();
                return View();
            }

            PopulateDropdownValues();
            return View();
        }

        /**
         * Handles input from Create Game form 
         */
        [HttpPost]
        public ActionResult CreateGame(CreateGameViewModel model)
        {
            // Confirm user is logged in (visitors can't create game)
            string email = User.Identity.GetUserName();
            Contact contact = _context.Contacts.FirstOrDefault(c => c.Email == email);

            if (contact == null)
            {
                ModelState.AddModelError("NoContact", "Please login or register to start a game.");
                PopulateDropdownValues();
                return View();
            }

            // Check model validation before doing anything
            if (!ModelState.IsValid)
            {
                PopulateDropdownValues();
                return View(model);
            }

            // Get start and end dates
            var dates = model.DateRange.Split(new char[] { '-' }, StringSplitOptions.RemoveEmptyEntries);
            var startDateTime = DateTime.Parse(dates[0], CultureInfo.InvariantCulture);
            var endDateTime = DateTime.Parse(dates[1], CultureInfo.CurrentCulture);

            // Return error to View if user picks different dates for start and end
            if (startDateTime.DayOfWeek != endDateTime.DayOfWeek)
            {
                ViewData.ModelState.AddModelError("DateRange", "Start date and end date must be same date.");
                PopulateDropdownValues();
                return View(model);
            }

            // Check if similar game already exists
            Game existingGame = CheckForExistingGame(model.VenueId, model.SportId, startDateTime);
            if (existingGame != null)
            {
                // TODO - Add link to existing game details later when that page is created
                ViewData.ModelState.AddModelError("GameExists", "A game already exists with this venue, sport, and time.");
                PopulateDropdownValues();
                return View(model);
            }

            // Get venue by ID and business hours for that venue
            Venue venue = _context.Venues.Find(model.VenueId);
            List<BusinessHours> venueHours = _context.BusinessHours.Where(b => b.VenueId == venue.VenueId).ToList();

            // Return error to View if the venue is not available
            bool isVenueAvailable = IsVenueAvailable(venueHours, startDateTime, endDateTime);
            if (!isVenueAvailable)
            {
                ViewData.ModelState.AddModelError("DateRange", $"Unfortunately, {venue.Name} is not available during the hours you chose.");
                PopulateDropdownValues();
                return View(model);
            }

            // All validation passed so add game to database 
            Game newGame = new Game
            {
                ContactId = contact.ContactId,
                GameStatusId = (int) GameStatusEnum.Open,
                VenueId = model.VenueId,
                SportId = model.SportId,
                StartTime = startDateTime,
                EndTime = endDateTime
            };

            _context.Games.Add(newGame);
            _context.SaveChanges();

            ViewBag.GameCreated = true;
            PopulateDropdownValues();
            return View();
        }

        /**
         * Routes user to page that shows a list of all current, open games
         */
        public ActionResult GameList()
        {
            ViewBag.Games = new SelectList(_context.Venues, "VenueId", "Name");
            // Get games that are open and that have not already passed and
            // order by games happening soonest
            List<Game> games = _context.Games
                .Where(g => g.GameStatusId == (int) GameStatusEnum.Open && g.StartTime > DateTime.Now)
                .OrderBy(g => g.StartTime).ToList();
            
            List<GameListViewModel> model = new List<GameListViewModel>();
            foreach (var game in games)
            {
                model.Add(new GameListViewModel
                {
                    GameId = game.GameId,
                    ContactName = _context.Contacts.Find(game.ContactId).Username,
                    Sport = _context.Sports.Find(game.SportId).SportName,
                    Venue = _context.Venues.Find(game.VenueId).Name,
                    StartDate = game.StartTime.ToString(),
                    EndDate = game.EndTime.ToString()
                });
            }

            return View(model);
        }

        /**
         * Routes user to GameDetails page to show details for single game
         */
        public ActionResult GameDetails(int id)
        {
            // TODO: This needs completing
            return View();
        }

        /**
         * Partial view that to display business hours on CreateGame page
         */
        [HttpGet]
        public PartialViewResult BusinessHoursByVenueId(int id)
        {
            // Map business hours
            List<BusinessHours> businessHours = _context.BusinessHours.Where(b => b.VenueId == id).ToList();
            List<BusinessHoursViewModel> model = new List<BusinessHoursViewModel>();

            foreach (var businessHour in businessHours)
            {
                var closeDateTime = new DateTime() + businessHour.CloseTime;
                var openDateTime = new DateTime() + businessHour.OpenTime;

                model.Add(new BusinessHoursViewModel
                {
                    DayOfWeek = Enum.GetName(typeof(DayOfWeek), businessHour.DayOfWeek),
                    CloseTime = closeDateTime.ToShortTimeString(),
                    OpenTime = openDateTime.ToShortTimeString()
                });
            }

            // Partial view displaying bids for specific item
            return PartialView("_BusinessHours", model);
        }


        public PartialViewResult GetGamesResult(int venueId)
        {
            //list of games found using venue ID
            List<Game> gameList = _context.Games.Where(x => x.VenueId == venueId).ToList();

            if (gameList.Count == 0)
            {
                ViewBag.ErrorMsg = "There are no games in the selected Venue";
            }

            //List using ViewModel to format how I like 
            List<GameListViewModel> model = new List<GameListViewModel>();


            //Find right data for each variable 
            foreach (var game in gameList)
            {
                model.Add(new GameListViewModel
                {
                    GameId = game.GameId,
                    ContactName = _context.Contacts.Find(game.ContactId).Username,
                    VenueId = venueId,
                    Sport = _context.Sports.Find(game.SportId).SportName,
                    Venue = _context.Venues.Find(game.VenueId).Name,
                    StartDate = game.StartTime.ToString(),
                    EndDate = game.EndTime.ToString()
                });
            }

            //returning it back to my Ajax js method
            return PartialView("_SearchByVenue", model);

        }

        [HttpGet]
        public ActionResult SearchBySports()
        {
            GameBySportViewModel model = new GameBySportViewModel();
            model.Sports = FetchSports();
            return View(model);
        }

        private static List<SelectListItem> FetchSports()
        {
            List<SelectListItem> items = new List<SelectListItem>();
            string constr = ConfigurationManager.ConnectionStrings["PickUpContext"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                string query = " SELECT SportName, SportID FROM Sport";
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    cmd.Connection = con;
                    con.Open();
                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            items.Add(new SelectListItem
                            {
                                Text = sdr["SportName"].ToString(),
                                Value = sdr["SportID"].ToString()
                            });
                        }
                    }
                    con.Close();
                }
            }

            return items;
        }

        [HttpPost]
        public ActionResult SearchBySports(GameBySportViewModel model)
        {
            model.Sports = FetchSports();
            var selectedItem = model.Sports.Find(p => p.Value == model.SportId.ToString());

            int selectedSportId = Convert.ToInt32(selectedItem.Value);

            if (selectedItem != null)
            {
                selectedItem.Selected = true;
                List<int> GameIdList = new List<int>();
                List<string> VenueNameList = new List<string>();
                List<DateTime> StartTimeList = new List<DateTime>();
                List<DateTime> EndTimeList = new List<DateTime>();
                List<string> ContactNameList = new List<string>();
                List<string> GameStatusList = new List<string>();

                List<Game> games = _context.Games.Where(s => s.SportId == selectedSportId).ToList();

                foreach (var item in games)
                {
                    GameIdList.Add(item.GameId);
                    VenueNameList.Add(_context.Venues.First(s => s.VenueId == item.VenueId).Name);
                    StartTimeList.Add(item.StartTime);
                    EndTimeList.Add(item.StartTime);                    
                    ContactNameList.Add( _context.Contacts.First(s => s.ContactId == item.ContactId).Username);
                    GameStatusList.Add(_context.GameStatuses.First(s => s.GameStatusId == item.GameStatusId).Status);
                    
                }
                model.GameId = GameIdList;
                model.VenueName = VenueNameList;
                model.StartTime = StartTimeList;
                model.EndTime = EndTimeList;
                model.ContactName = ContactNameList;
                model.GameStatus = GameStatusList;

                ViewBag.Message = "True";
            }

            return View(model);
        }



        /**
 * Helper methods
 */

        public void PopulateDropdownValues()
        {
            ViewBag.Venues = _context.Venues.ToList().ToDictionary(v => v.VenueId, v => v.Name);
            ViewBag.Sports = _context.Sports.ToList().ToDictionary(s => s.SportID, s => s.SportName);
        }

        public bool IsVenueAvailable(List<BusinessHours> venueHours, DateTime startDateTime, DateTime endDateTime)
        {
            // If no business hours then venue has no hours and is therefore not available
            if (venueHours == null) return false;

            // Only checking start date because games should not span over a day 
            DayOfWeek startDate = startDateTime.DayOfWeek;
            BusinessHours venueOpenDate = venueHours.FirstOrDefault(x => x.DayOfWeek == (int)startDate);

            // Venue is open that date, check timeframes
            if (venueOpenDate != null)
            {
                TimeSpan startTime = startDateTime.TimeOfDay;
                TimeSpan endTime = endDateTime.TimeOfDay;

                // Change midnight to 11:59 PM for accurate time comparisons
                if (venueOpenDate.CloseTime == new TimeSpan(00, 00, 00))
                    venueOpenDate.CloseTime = new TimeSpan(23, 59, 00);

                // Ensure both start and end times are within range
                if (startTime > venueOpenDate.OpenTime && startTime < venueOpenDate.CloseTime)
                {
                    if (endTime > venueOpenDate.OpenTime && endTime < venueOpenDate.CloseTime) return true;
                }
            }

            return false;
        }

        public Game CheckForExistingGame(int venueId, int sportId, DateTime startDateTime)
        {
            // Check for all games that are happening at same venue
            List<Game> gamesAtVenue = _context.Games.Where(g => g.VenueId == venueId).ToList();
            if (gamesAtVenue.Count <= 0) return null;

            // Check for all games happening at that venue with same sport
            List<Game> sportsAtVenue = gamesAtVenue.Where(g => g.SportId == sportId).ToList();
            if (sportsAtVenue.Count <= 0) return null;

            // There are existing games with same sport and venue so check starting time
            foreach (var game in sportsAtVenue)
            {
                if (startDateTime >= game.StartTime && startDateTime <= game.EndTime)
                {
                    // If we get here, the new game will overlap with an existing game
                    // Check if status is Open and if so, return that game
                    if (game.GameStatusId == (int)GameStatusEnum.Open)
                    {
                        return game;
                    }
                }
            }

            return null;

        }

    }
}