using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security.Google;
using PickUpSports.DAL;
using PickUpSports.Interface;
using PickUpSports.Models.DatabaseModels;
using PickUpSports.Models.ViewModel;
using PickUpSports.Models.ViewModel.VenueOwnerController;
using RestSharp.Extensions;

namespace PickUpSports.Controllers
{
    public class VenueOwnerController : Controller
    {
        private readonly PickUpContext _context;
        private readonly IGMailService _gMailer;

        public VenueOwnerController(PickUpContext context)
        {
            _context = context;
        }

        public VenueOwnerController(PickUpContext context, IGMailService gMailer)
        {
            _context = context;
            _gMailer = gMailer;
        }

        [HttpGet]
        public ActionResult Create()
        {
            //Return drop down of the venues
            PopulateDropdownValues();
            return View();
        }

        [HttpPost]
        public ActionResult Create(CreateVenueOwnerViewModel model)
        {
            PopulateDropdownValues();

            //if model state is invalid return
            if (!ModelState.IsValid)
            {
                PopulateDropdownValues();
                return View(model);
            }
            
            //Find venue with the view model id
            Venue venue = _context.Venues.Find(int.Parse(model.VenueName));

            //If the venue already has an owner, error 
            if (VenueHasOwner(venue))
            {
                ViewData.ModelState.AddModelError("OwnerExists", "There is already an owner for this Venue");
                PopulateDropdownValues();
                return View(model);
            }

            //Creating new owner with the values from the view
            VenueOwner owner = new VenueOwner
            {
                VenueOwnerId = model.VenueOwnerId,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                Phone = model.PhoneNumber,
                CompanyName = model.CompanyName,
                SignUpDate = DateTime.Now,
                VenueId = _context.Venues.FirstOrDefault(x => x.Name == model.VenueName).VenueId
            };

            //Add the owner to the table, and save changes
            _context.VenueOwners.Add(owner);
            _context.SaveChanges();

            //Take them to the page that shows the details that they just added 
            return RedirectToAction("Detail", new {id = model.VenueOwnerId});
        }

        public bool VenueHasOwner(Venue venue)
        {
            //Find the owner using the venue ID, again could be simplified using repo patterns
            VenueOwner owner = _context.VenueOwners.FirstOrDefault(x => x.VenueId == venue.VenueId);

            //if there is not an owner it would be null so return false
            if (owner == null) return false;

            //else there is an owner and the value is not null so return true 
            return true;
        }

        public ActionResult Detail(int id)
        {
            //Get the current logged in user - will be implemented in the future when the log in function/ page is complete
            //string ownerEmail = User.Identity.GetUserName();

            //Finds the owner with the current logged in user email
            //VenueOwner owner = _context.VenueOwners.FirstOrDefault(x => x.Email == ownerEmail);

            //Finding the owner using the id 
            VenueOwner owner = _context.VenueOwners.Find(id);


            //If the owner has not created an account yet, redirect them to the create page
            if (owner == null) return RedirectToAction("Create", "VenueOwner");

            //Create the view model using values of the owner if not null
            CreateVenueOwnerViewModel model = new CreateVenueOwnerViewModel()
            {
                VenueOwnerId = owner.VenueOwnerId,
                FirstName = owner.FirstName,
                LastName = owner.LastName,
                Email = owner.Email,
                PhoneNumber = owner.Phone,
                CompanyName = owner.CompanyName,
                SignUpDate = owner.SignUpDate,
                VenueName = _context.Venues.Find(owner.VenueId).Name
            };

            //Return it back to the view
            return View(model);
        }

        /**
         * Helper method that creates the dropdown of the venues
         */
        public void PopulateDropdownValues()
        {
            ViewBag.Venues = _context.Venues.ToList().ToDictionary(v => v.VenueId, v => v.Name);
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            //Bad request if value is 0
            if (id == 0) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            //Find venue owner using the id
            VenueOwner owner = _context.VenueOwners.Find(id);

            //Initialize view model values using the owner values
            CreateVenueOwnerViewModel model = new CreateVenueOwnerViewModel()
            { 
                VenueOwnerId = owner.VenueOwnerId,
                FirstName = owner.FirstName,
                LastName = owner.LastName,
                Email = owner.Email,
                PhoneNumber = owner.Phone,
                CompanyName = owner.CompanyName,
                SignUpDate = owner.SignUpDate,
                VenueName = _context.Venues.Find(owner.VenueId).Name
            };

            //Return view back
            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(CreateVenueOwnerViewModel model)
        {
            //Find the existing owner withe the Id
            VenueOwner existingOwner = _context.VenueOwners.Find(model.VenueOwnerId);

            //Add the changed values to the existing owner
            existingOwner.VenueOwnerId = model.VenueOwnerId;
            existingOwner.FirstName = model.FirstName;
            existingOwner.LastName = model.LastName;
            existingOwner.Email = model.Email;
            existingOwner.Phone = model.PhoneNumber;
            existingOwner.CompanyName = model.CompanyName;

            //save the changes
            _context.Entry(existingOwner).State = EntityState.Modified;
            _context.SaveChanges();

            //Redirect to the details page 
            return RedirectToAction("Detail", "VenueOwner", new {id = existingOwner.VenueOwnerId});
        }

        [HttpPost]
        public ActionResult MessageOwner()
        {
            
        }
    }
}
