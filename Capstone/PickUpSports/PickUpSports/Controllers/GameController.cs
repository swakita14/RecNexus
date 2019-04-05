using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using PickUpSports.DAL;
using PickUpSports.Models.DatabaseModels;
using PickUpSports.Models.ViewModel.GameController;

namespace PickUpSports.Controllers
{
    public class GameController : Controller
    {
        private readonly PickUpContext _context;

        public GameController(PickUpContext context)
        {
            _context = context;
        }

        public ActionResult CreateGame()
        {
            // Populate drowndown box values
            ViewBag.Venues = _context.Venues.ToList().ToDictionary(v => v.VenueId, v => v.Name);
            ViewBag.Sports = _context.Sports.ToList().ToDictionary(s => s.SportID, s => s.SportName);

            return View();
        }

        [HttpPost]
        public ActionResult CreateGame(CreateGameViewModel model)
        {
            if (ModelState.IsValid) return View(model);

            // Get venue by ID and business hours for that venue
            Venue venue = _context.Venues.Find(model.VenueId);
            List<BusinessHours> venueHours = _context.BusinessHours.Where(b => b.VenueId == venue.VenueId).ToList();

            // Get start and end dates
            var dates = model.DateRange.Split(new char[] { '-' }, StringSplitOptions.RemoveEmptyEntries);
            var startDateTime = DateTime.Parse(dates[0], CultureInfo.CurrentCulture);
            var endDateTime = DateTime.Parse(dates[1], CultureInfo.CurrentCulture);

            

            return View();
        }

        public bool VenueIsAvailable(List<BusinessHours> venueHours, DateTime startDateTime, DateTime endDateTime)
        {
            // If no business hours then venue has no hours and is therefore not available
            if (venueHours == null) return false;

            // Only checking start date because games should not span over a day 
            DayOfWeek startDate = startDateTime.DayOfWeek;
            BusinessHours venueOpenDate = venueHours.FirstOrDefault(x => x.DayOfWeek == (int) startDate);

            // Venue is open that date, check timeframes
            if (venueOpenDate != null)
            {
                TimeSpan startTime = startDateTime.TimeOfDay;
                TimeSpan endTime = endDateTime.TimeOfDay;

                // Ensure both start and end times are within range
                if (startTime > venueOpenDate.OpenTime && startTime < venueOpenDate.CloseTime)
                {
                    if (endTime > venueOpenDate.OpenTime && endTime < venueOpenDate.CloseTime) return true;
                }
            }

            return false;
        }
    }
}