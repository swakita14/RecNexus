using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using PickUpSports.Interface;
using PickUpSports.Models.DatabaseModels;
using PickUpSports.Models.ViewModel.ContactController;

namespace PickUpSports.Controllers
{
    [Authorize]
    public class ContactController : Controller
    {
        private readonly IContactService _contactService;

        public ContactController(IContactService contactService)
        {
            _contactService = contactService;
        }


        public ActionResult Details(int? id)
        {
            string newContactEmail = User.Identity.GetUserName();

            Contact contact = _contactService.GetContactByEmail(newContactEmail);

            // If username is null, profile was never set up
            if (contact == null || contact.Username == null) return RedirectToAction("Create", "Contact");

            return View(contact);
        }

        public ActionResult Create()
        {
            ViewBag.States = PopulateStatesDropdown();
            return View();
        }

        // POST: Contacts/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CreateContactViewModel model)
        {
            ViewBag.Error = "";
            if (!ModelState.IsValid)
            {
                ViewBag.States = PopulateStatesDropdown();
                return View(model);
            }

            //create user 
            string email = User.Identity.GetUserName();
            Debug.Write(email);

            Contact newContact = new Contact
            {
                ContactId = model.ContactId,
                Username = model.Username,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = email,
                PhoneNumber = model.PhoneNumber,
                Address1 = model.Address1,
                Address2 = model.Address2,
                City = model.City,
                State = model.State,
                ZipCode = model.ZipCode
            };

            if (_contactService.UsernameIsTaken(model.Username))
            {
                ModelState.AddModelError("Username", "Username already taken");
                ViewBag.States = PopulateStatesDropdown();
                return View(model);
            }

            _contactService.CreateContact(newContact);
            return RedirectToAction("Details", new {id = model.ContactId});

        }

        // GET: Contacts/Edit/5
        public ActionResult Edit(int? id)
        {
            ViewBag.States = PopulateStatesDropdown();

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Contact contact = _contactService.GetContactById((int) id);

            if (contact == null) return HttpNotFound();

            EditContactViewModel model = new EditContactViewModel
            {
                ContactId = contact.ContactId,
                Username = contact.Username,
                FirstName = contact.FirstName,
                LastName = contact.LastName,
                Email = contact.Email,
                PhoneNumber = contact.PhoneNumber,
                Address1 = contact.Address1,
                Address2 = contact.Address2,
                City = contact.City,
                State = contact.State,
                ZipCode = contact.ZipCode,
                HasPublicProfile = contact.HasPublicProfile
            };

            return View(model);
        }

        // POST: Contacts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(EditContactViewModel model)
        {
            if (ModelState.IsValid) return View(model);
            Contact existing = _contactService.GetContactById(model.ContactId);

            existing.FirstName = model.FirstName;
            existing.LastName = model.LastName;
            existing.PhoneNumber = model.PhoneNumber;
            existing.Address1 = model.Address1;
            existing.Address2 = model.Address2;
            existing.City = model.City;
            existing.State = model.State;
            existing.ZipCode = model.ZipCode;
            existing.HasPublicProfile = model.HasPublicProfile;

            _contactService.EditContact(existing);

            return RedirectToAction("Details");
        }

        // POST: Contacts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(EditContactViewModel model)
        {
            return RedirectToAction("RemoveAccount", "Account", new { id = model.ContactId});
        }

        /**
         * Endpoint that routes to public profile view. Should take in a Contact ID 
         */
        [HttpGet]
        public ActionResult Profile(int id)
        {
            var model = new ProfileViewModel();
            var contact = _contactService.GetContactById(id);
            if (contact == null) throw new ArgumentNullException($"Contact ID {id} does not exist.");

            model.Username = contact.Username;
            model.ContactId = contact.ContactId;
            model.UserAllowsPublicProfile = contact.HasPublicProfile;
            return View(model);
        }

        public ActionResult GetSportPreferences(int contactId, bool isPublicProfileView)
        {
            var model = new SportPreferenceViewModel
            {
                ContactId = contactId
            };

            model.IsPublicProfileView = isPublicProfileView;

            var results = _contactService.GetUserSportPreferences(contactId);
            if (results == null) return PartialView("../SportPreferences/_SportPreferences", model);

            model.SportName = results;
            return PartialView("../SportPreferences/_SportPreferences", model);
        }

        public ActionResult GetTimePreferences(int contactId, bool isPublicProfileView)
        {
            var model = new TimePreferenceListViewModel
            {
                ContactId = contactId,
                TimePreferences = new List<TimePreferenceViewModel>()           
            };

            model.IsPublicProfileView = isPublicProfileView;

            var timePreferences = _contactService.GetUserTimePreferences(contactId);
            if (timePreferences == null) return PartialView("../TimePreferences/_TimePreferences", model);

            foreach (var timePreference in timePreferences)
            {
                model.TimePreferences.Add(new TimePreferenceViewModel
                {
                    DayOfWeek = (DayOfWeek) timePreference.DayOfWeek,
                    BeginTime = timePreference.BeginTime,
                    EndTime = timePreference.EndTime
                });
            }

            return PartialView("../TimePreferences/_TimePreferences", model);
        }

        private Dictionary<string, string> PopulateStatesDropdown()
        {
            var states = new Dictionary<string, string>();

            states.Add("AL", "Alabama");
            states.Add("AK", "Alaska");
            states.Add("AZ", "Arizona");
            states.Add("AR", "Arkansas");
            states.Add("CA", "California");
            states.Add("CO", "Colorado");
            states.Add("CT", "Connecticut");
            states.Add("DE", "Delaware");
            states.Add("DC", "District of Columbia");
            states.Add("FL", "Florida");
            states.Add("GA", "Georgia");
            states.Add("HI", "Hawaii");
            states.Add("ID", "Idaho");
            states.Add("IL", "Illinois");
            states.Add("IN", "Indiana");
            states.Add("IA", "Iowa");
            states.Add("KS", "Kansas");
            states.Add("KY", "Kentucky");
            states.Add("LA", "Louisiana");
            states.Add("ME", "Maine");
            states.Add("MD", "Maryland");
            states.Add("MA", "Massachusetts");
            states.Add("MI", "Michigan");
            states.Add("MN", "Minnesota");
            states.Add("MS", "Mississippi");
            states.Add("MO", "Missouri");
            states.Add("MT", "Montana");
            states.Add("NE", "Nebraska");
            states.Add("NV", "Nevada");
            states.Add("NH", "New Hampshire");
            states.Add("NJ", "New Jersey");
            states.Add("NM", "New Mexico");
            states.Add("NY", "New York");
            states.Add("NC", "North Carolina");
            states.Add("ND", "North Dakota");
            states.Add("OH", "Ohio");
            states.Add("OK", "Oklahoma");
            states.Add("OR", "Oregon");
            states.Add("PA", "Pennsylvania");
            states.Add("RI", "Rhode Island");
            states.Add("SC", "South Carolina");
            states.Add("SD", "South Dakota");
            states.Add("TN", "Tennessee");
            states.Add("TX", "Texas");
            states.Add("UT", "Utah");
            states.Add("VT", "Vermont");
            states.Add("VA", "Virginia");
            states.Add("WA", "Washington");
            states.Add("WV", "West Virginia");
            states.Add("WI", "Wisconsin");
            states.Add("WY", "Wyoming");

            return states;
        }
    }
}
