using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Mail;
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
        private readonly IVenueOwnerService _venueOwnerService;
        private readonly IVenueService _venueService; 

        public VenueOwnerController(PickUpContext context)
        {
            _context = context;
        }

        public VenueOwnerController(PickUpContext context, IGMailService gMailer, IVenueOwnerService venueOwnerService, IVenueService venueService)
        {
            _context = context;
            _gMailer = gMailer;
            _venueOwnerService = venueOwnerService;
            _venueService = venueService;
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
            Venue venue = _venueService.GetVenueById(int.Parse(model.VenueName));

            //If the venue already has an owner, error 
            if (_venueOwnerService.VenueHasOwner(venue))
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
                VenueId = venue.VenueId
            };

            //Add the owner to the table, and save changes
            _venueOwnerService.AddVenueOwner(owner);

            //Take them to the page that shows the details that they just added 
            return RedirectToAction("Detail", new {id = model.VenueOwnerId});
        }

        [Authorize]
        public ActionResult Detail(int id)
        {
            //Get the current logged in user - will be implemented in the future when the log in function/ page is complete
            //string ownerEmail = User.Identity.GetUserName();

            //Finds the owner with the current logged in user email
            //VenueOwner owner = _venueOwnerService.GetVenueOwnerByEmail(ownerEmail);

            //Finding the owner using the id 
            VenueOwner owner = _venueOwnerService.GetVenueOwnerById(id);


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
                VenueName = _venueService.GetVenueNameById(owner.VenueId)
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
            VenueOwner owner = _venueOwnerService.GetVenueOwnerById(id);

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
            VenueOwner existingOwner = _venueOwnerService.GetVenueOwnerById(model.VenueOwnerId);

            //Add the changed values to the existing owner
            existingOwner.VenueOwnerId = model.VenueOwnerId;
            existingOwner.FirstName = model.FirstName;
            existingOwner.LastName = model.LastName;
            existingOwner.Email = model.Email;
            existingOwner.Phone = model.PhoneNumber;
            existingOwner.CompanyName = model.CompanyName;

            //save the changes
            _venueOwnerService.EditVenueOwner(existingOwner);

            //Redirect to the details page 
            return RedirectToAction("Detail", "VenueOwner", new {id = existingOwner.VenueOwnerId});
        }

        [HttpPost]
        public ActionResult MessageOwner(CreateVenueOwnerViewModel model)
        {
            //Get the current logged in users email
            string currUserEmail = User.Identity.GetUserName();

            //Find the Venue owner using the Id
            VenueOwner owner = _venueOwnerService.GetVenueOwnerById(model.VenueOwnerId);

            string subject = "Message from User Concerning Your Venue: " + model.MessageSubject;
            string body = "<b>" + currUserEmail + "</b> wrote: <i>" + model.MessageBody + "</i>";

            //Create a new message with the inputs
            MailMessage mail = new MailMessage(_gMailer.GetEmailAddress(), owner.Email)
            {
                Subject = subject,
                Body = body
            };

            mail.IsBodyHtml = true;

            //Send Message
            _gMailer.Send(mail);

            return RedirectToAction("Detail", "VenueOwner", new { id = model.VenueOwnerId });
        }
    }
}
