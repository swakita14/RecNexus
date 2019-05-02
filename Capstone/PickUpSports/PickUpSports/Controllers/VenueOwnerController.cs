using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PickUpSports.DAL;
using PickUpSports.Models.DatabaseModels;
using PickUpSports.Models.ViewModel;
using RestSharp.Extensions;

namespace PickUpSports.Controllers
{
    public class VenueOwnerController : Controller
    {
        private readonly PickUpContext _context;

        public VenueOwnerController(PickUpContext context)
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult Create()
        {
            PopulateDropdownValues();
            return View();
        }

        [HttpPost]
        public ActionResult Create(CreateVenueOwnerViewModel model)
        {

            return RedirectToAction("Detail", new {id = model.VenueOwnerId});
        }

        public ActionResult Detail(int id)
        {

            //return View(model);
        }

        public void PopulateDropdownValues()
        {
            ViewBag.Venues = _context.Venues.ToList().ToDictionary(v => v.VenueId, v => v.Name);
        }
    }
}
