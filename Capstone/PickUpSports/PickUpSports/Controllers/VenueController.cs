using System.Collections.Generic;
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
            // Get places
            List<Result> places = await _placesApi.GetPlaces();

            foreach (var place in places)
            {
                // Check if we already have this venue in database
                Venue existingVenue = _context.Venues.FirstOrDefault(v => v.GooglePlaceId == place.PlaceId);

                // If not in database, add it
                if (existingVenue == null)
                {
                    Venue venue = new Venue();
                    venue.GooglePlaceId = place.PlaceId;
                    venue.Name = place.Name;
                    _context.Venues.Add(venue);
                    _context.SaveChanges();
                }
            }

            return View();
        }
    }
}