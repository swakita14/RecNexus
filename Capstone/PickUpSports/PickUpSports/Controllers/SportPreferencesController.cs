using System;
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

            return View(_context.SportPreferences.Where(x => x.ContactID == contact.ContactId).ToList());           
        }

        // GET: SportPreferences/Create
        public ActionResult Create()
        {
            // Contact contact = _context.Contacts.Find(id);
            //Identify the person using email
            string newContactEmail = User.Identity.GetUserName();

            //find the contact
            Contact contact = _context.Contacts.FirstOrDefault(x => x.Email == newContactEmail);
            var model = new List<CreateSportPreferenceViewModel>
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
        public ActionResult Create(List<CreateSportPreferenceViewModel> model)
        {
            string email = User.Identity.GetUserName();
            //string name =;

            Contact contact = _context.Contacts.FirstOrDefault(x=>x.Email==email);
            //Sport sport = _context.Sports.FirstOrDefault(x => x.SportName==name);

            SportPreference sportPreference = new SportPreference
            {
                ContactID = contact.ContactId,
                //the value is the id
                SportID = model.SportID      
            };
            _context.SportPreferences.Add(sportPreference);
            _context.SaveChanges();

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
