using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using PickUpSports.Interface;
using PickUpSports.Models;
using PickUpSports.Models.DatabaseModels;
using PickUpSports.Models.ViewModel.VenueOwnerController;

namespace PickUpSports.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly IVenueService _venueService;
        private readonly IVenueOwnerService _venueOwnerService;
        private ApplicationUserManager _userManager;

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        public AdminController(IVenueService venueService, IVenueOwnerService venueOwnerService)
        {
            _venueService = venueService;
            _venueOwnerService = venueOwnerService;
        }

        public AdminController(IVenueService venueService, IVenueOwnerService venueOwnerService, ApplicationUserManager userManager)
        {
            _venueService = venueService;
            _venueOwnerService = venueOwnerService;
            UserManager = userManager;
        }


        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AddVenueOwner()
        {
            PopulateDropdownValues();
            return PartialView("_AddVenueOwner");
        }

        [HttpPost]
        public ActionResult AddVenueOwner(CreateVenueOwnerViewModel model)
        {
            PopulateDropdownValues();

            //if model state is invalid return
            if (!ModelState.IsValid)
            {
                PopulateDropdownValues();
                return PartialView("_AddVenueOwner", model);
            }

            //Find venue with the view model id
            Venue venue = _venueService.GetVenueById(model.VenueId);

            //If the venue already has an owner, error 
            if (_venueOwnerService.VenueHasOwner(venue))
            {
                ViewData.ModelState.AddModelError("OwnerExists", "There is already an owner for this Venue");
                PopulateDropdownValues();
                return PartialView("_AddVenueOwner", model);
            }
            
            // Adding owner to AspNetUsers table
            var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
            var result = UserManager.Create(user, model.TemporaryPassword);

            //Creating new owner with the values from the view
            VenueOwner owner = new VenueOwner
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                Phone = model.PhoneNumber,
                CompanyName = model.CompanyName,
                SignUpDate = DateTime.Now,
                VenueId = venue.VenueId
            };
            //Add the owner to the table, and save changes
            _venueOwnerService.AddVenueOwner(owner);

            //Take them to the page that shows the details that they just added 
            return PartialView("_AddVenueOwner", model);
        }

        /*
         * Helper method that creates the dropdown of the venues
         */
        public void PopulateDropdownValues()
        {
            ViewBag.Venues = _venueService.GetAllVenues().ToDictionary(v => v.VenueId, v => v.Name);
        }
    }
}