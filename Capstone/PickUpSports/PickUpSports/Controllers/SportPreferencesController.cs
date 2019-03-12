﻿using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using PickUpSports.DAL;
using PickUpSports.Models.DatabaseModels;
using PickUpSports.Models.ViewModel;

namespace PickUpSports.Controllers
{
    public class SportPreferencesController : Controller
    {
        private readonly PickUpContext _context;

        public SportPreferencesController(PickUpContext context)
        {
            _context = context;
        }        

        // GET: SportPreferences/Details
        public ActionResult Details()
        {
            //Identify the person using email
            string email = User.Identity.GetUserName();

            //find the contact
            Contact contact = _context.Contacts.FirstOrDefault(x => x.Email == email);

            SportPreferenceViewModel model = new SportPreferenceViewModel();

            model.ContactID = contact.ContactId;
            model.ContactUsername = contact.Username;

            // find the sportpreferences for the user
            List<SportPreference> sportPrefs = _context.SportPreferences.Where(s => s.ContactID == contact.ContactId).ToList();
            
            //for each preference of the user, get the sport name
            List<string> sportNameList = new List<string>();

            foreach (var sportPref in sportPrefs)
            {
                List<Sport> sports = _context.Sports.Where(x => x.SportID == sportPref.SportID).ToList();
                foreach (var sport in sports)
                {
                    sportNameList.Add(sport.SportName);
                }              
            }
            //add all the sport name to the list back to the view
            model.SportName=sportNameList;
            return View(model);
        }
        [HttpGet]
        // GET: SportPreferences/Create    
        public ActionResult Create()
        {
            //Identify the person using email
            string newContactEmail = User.Identity.GetUserName();

            //find the contact
            Contact contact = _context.Contacts.FirstOrDefault(x => x.Email == newContactEmail);

            // Create view model 
            var model = new CreateSportPreferenceViewModel
            {
                ContactId = contact.ContactId,
                ContactUsername = contact.Username,
                SportPreferenceCheckboxes = new List<SelectSportPreferenceViewModel>()
            };

            List<Sport> sports = _context.Sports.ToList();

            // For each sport in the database, add an object to the Sport Preferences list
            // in the view model which will translate to checkboxes on the view
            foreach (var sport in sports)
            {
                // Initialize viewmodel that will be used for checkbox
                SelectSportPreferenceViewModel sportPrefCheckbox = new SelectSportPreferenceViewModel
                {
                    SportId = sport.SportID,
                    SportName = sport.SportName
                };

                // Check if user has preference set for this specific sport
                SportPreference sportPref = _context.SportPreferences.FirstOrDefault(s => s.ContactID == contact.ContactId && s.SportID == sport.SportID);

                // If no preference for sport, set IsSelected to false because it's not a preference for this user
                if (sportPref == null) sportPrefCheckbox.IsSelected = false;
                else sportPrefCheckbox.IsSelected = true;

                // Add to list of checkboxes
                model.SportPreferenceCheckboxes.Add(sportPrefCheckbox);

            }
            return View(model);
        }

        [HttpPost]
        public ActionResult Create(CreateSportPreferenceViewModel model)
        {
            // Update database with any updated values from checkboxes
            foreach (var preference in model.SportPreferenceCheckboxes)
            {
                // check for existing sport preference
                SportPreference existing = _context.SportPreferences.FirstOrDefault(s => s.ContactID == model.ContactId && s.SportID == preference.SportId);

                // Sport preference does not exist and user has selected it so add to database
                if (existing == null && preference.IsSelected)
                {
                    _context.SportPreferences.Add(new SportPreference
                    {
                        ContactID = model.ContactId,
                        SportID = preference.SportId
                    });
                }

                // Sport preference exists but user no longer wants it to be set so remove
                if (existing != null && !preference.IsSelected)
                {
                    _context.SportPreferences.Remove(existing);
                }

                _context.SaveChanges();
            }

            return RedirectToAction("Details", "SportPreferences");
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
