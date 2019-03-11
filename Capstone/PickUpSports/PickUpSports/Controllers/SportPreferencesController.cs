using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
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

        // GET: SportPreferences/Details/5
        public ActionResult Details()
        {
            //Identify the person using email
            string email = User.Identity.GetUserName();

            //find the contact
            Contact contact = _context.Contacts.FirstOrDefault(x => x.Email == email);

            SportPreferenceViewModel model = new SportPreferenceViewModel();

            model.ContactID = contact.ContactId;
            model.ContactUsername = contact.Username;
            
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

            var model = new CreateSportPreferenceViewModel
            {
                ContactID = contact.ContactId,
                ContactUsername = contact.Username
            };
            return View(model);
        }

        // POST: SportPreferences/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Create(CreateSportPreferenceViewModel model)
        {
            string email = User.Identity.GetUserName();

            Contact contact = _context.Contacts.FirstOrDefault(x=>x.Email==email);

            /* SportPreference sportPreference = new SportPreference
             {
                 SportID=model.SportID,
                 ContactID=model.ContactID,           
             };

             _context.SportPreferences.Add(sportPreference);
             _context.SaveChanges();

             return RedirectToAction("Details", "SportPreferences");*/

            //List<Sport> sports = _context.Reviews.Where(r => r.VenueId == id).ToList();
            List<Sport> sports = _context.Sports.Where(r=>r.SportID==model.SportID).ToList();

            model.Sports = new List<Sport>();

            List<Sport> tempList = new List<Sport>();
            foreach (var sport in sports)
            {
                Sport sportprefer = new Sport
                {
                    SportID=sport.SportID,
                    SportName=sport.SportName
                };

                SportPreference sportPreference = new SportPreference
                {
                    SportID = model.SportID,
                    ContactID = model.ContactID,
                };
                tempList.Add(sportprefer);
                _context.SportPreferences.Add(sportPreference);
                _context.SaveChanges();
            }
            
            // Order reviews newest to oldest
            model.Sports = tempList.ToList();
            return RedirectToAction("Details", "SportPreferences");
        }
             

        // GET: SportPreferences/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SportPreference sportPreference = _context.SportPreferences.Find(id);
            if (sportPreference == null)
            {
                return HttpNotFound();
            }
            return View(sportPreference);
        }

        // POST: SportPreferences/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "SportPrefID,ContactID,SportID")] SportPreference sportPreference)
        {
            if (ModelState.IsValid)
            {
                _context.Entry(sportPreference).State = EntityState.Modified;
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(sportPreference);
        }

        // GET: SportPreferences/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SportPreference sportPreference = _context.SportPreferences.Find(id);
            if (sportPreference == null)
            {
                return HttpNotFound();
            }
            return View(sportPreference);
        }

        // POST: SportPreferences/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SportPreference sportPreference = _context.SportPreferences.Find(id);
            _context.SportPreferences.Remove(sportPreference);
            _context.SaveChanges();
            return RedirectToAction("Index");
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
