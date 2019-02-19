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

namespace PickUpSports.Controllers
{
    public class ContactsController : Controller
    {
        private ContactContext db = new ContactContext();

        // GET: Contacts
        public ActionResult Index()
        {
            return View(db.Contacts.ToList());
        }

        // GET: Contacts/Details/5
        public ActionResult Details(int? id)
        {
            string newContact_Email = User.Identity.GetUserName();

            Contact newPerson = new Contact()
            {
                Email = newContact_Email
            };
            db.Contacts.Add(newPerson);
            db.SaveChanges();


            Contact contact = db.Contacts.Where(x => x.Email == newContact_Email).FirstOrDefault();
            if (contact == null)
            {
                return HttpNotFound();
            }
            //We have to switch the username in the DB from NOT NULL to nullable 
            //If username not chosen, it's time to create one for you buddy 
            else if (contact.Username == null)
            {
                return RedirectToAction("Create", "Contacts");
            }
            return View(contact);
        }

        // GET: Contacts/Create
        public ActionResult Create()
        {
            string email = User.Identity.GetUserName();
            Contact model = new Contact()
            {
                ContactId = (int)db.Contacts.FirstOrDefault(x => x.Email == email)?.ContactId
            };
            return View();
        }

        // POST: Contacts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Contact contact)
        {
            if (!db.Contacts.Where(u => u.Username == contact.Username).Any())
            {
                Contact newContact = new Contact()
                {
                    ContactId = contact.ContactId,
                    Username = contact.Username,
                    FirstName = contact.FirstName,
                    LastName = contact.LastName,
                    PhoneNumber = contact.PhoneNumber,
                    Address1 = contact.Address1,
                    Address2 = contact.Address2,
                    City = contact.City,
                    State = contact.State,
                    ZipCode = contact.ZipCode
                };


                if (ModelState.IsValid)
                {
                    db.Contacts.Add(contact);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            else
            {
                Debug.Write("Hello World");

            }

            return View(contact);
        }

        // GET: Contacts/Edit/5
        public ActionResult Edit(string id)
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
        public ActionResult Edit([Bind(Include = "ContactId,Username,FirstName,LastName,Email,PhoneNumber,Address1,Address2,City,State,ZipCode")] Contact contact)
        {
            if (ModelState.IsValid)
            {
                db.Entry(contact).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(contact);
        }

        // GET: Contacts/Delete/5
        public ActionResult Delete(string id)
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
        public ActionResult DeleteConfirmed(string id)
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
