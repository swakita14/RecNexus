using System.Linq;
using System.Web.Mvc;
using PickUpSports.DAL;
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
            // Still need logic here to create the game
            return View();
        }
    }
}