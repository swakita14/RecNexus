using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using PickUpSports.Models;
using PickUpSports.Models.ViewModel;

namespace PickUpSports.Controllers
{
    public class ContactsController : Controller
    {
        private PickUpContext db = new PickUpContext();

        // GET: Contacts
        public ActionResult Index()
        {
            return View(db.Contacts.ToList());
        }

        // GET: Contacts/Details/5
        public ActionResult Details(int? id)
        {
            string newContact_Email = User.Identity.GetUserName();

            //check if user email is already in db, if not redirect him to creating info for contact table
            if (!db.Contacts.Where(u => u.Email == newContact_Email).Any())
            {
                return RedirectToAction("Create", "Contacts");
            }

            Contact contact = db.Contacts.Where(x => x.Email == newContact_Email).FirstOrDefault();

            if (contact == null)
            {
                return HttpNotFound();
            }

            return View(contact);
        }

        // GET: Contacts/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Contacts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CreateContactViewModel model)
        {
            ViewBag.Error = "";
            if (ModelState.IsValid) return View(model);
            //create user 
            string email = User.Identity.GetUserName();
            Debug.Write(email);

                Contact newContact = new Contact()
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

                if (db.Contacts.Where(u => u.Username == model.Username).Any())
                {
                    ViewBag.Error = "Username Already Taken";
                    return View(model);
                }

            //Need to find out why its not being valid
            db.Contacts.Add(newContact);
            db.SaveChanges();
            return RedirectToAction("Details");

        }

        // GET: Contacts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Contact contact = db.Contacts.Find(id);
            if (contact == null)
            {
                return HttpNotFound();
            }
            return View(contact);
        }

        // POST: Contacts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(EditContactViewModel model)
        {
            if (ModelState.IsValid) return View(model);

            string email = User.Identity.GetUserName();

            Contact newContact = new Contact()
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

            if (db.Contacts.Where(u => u.Username == model.Username).Any())
            {
                ViewBag.Error = "Username Already Taken";
                return View(newContact);
            }

            db.Entry(newContact).State = EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("Details");

        }

        // GET: Contacts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Contact contact = db.Contacts.Find(id);
            if (contact == null)
            {
                return HttpNotFound();
            }
            return View(contact);
        }

        // POST: Contacts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int? id)
        {
            Contact contact = db.Contacts.Find(id);
            db.Contacts.Remove(contact);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
