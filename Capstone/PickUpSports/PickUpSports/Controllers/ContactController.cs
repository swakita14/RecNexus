using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using PickUpSports.DAL;
using PickUpSports.Models;
using PickUpSports.Models.ViewModel;

namespace PickUpSports.Controllers
{
    public class ContactController : Controller
    {
        private readonly PickUpContext _context = new PickUpContext();

        // GET: Contacts
        public ActionResult Index()
        {
            return View(_context.Contacts.ToList());
        }

        // GET: Contact/Details/5
        public ActionResult Details(int? id)
        {
            string newContactEmail = User.Identity.GetUserName();

            Contact contact = _context.Contacts.FirstOrDefault(x => x.Email == newContactEmail);

            // If username is null, profile was never set up
            if (contact.Username == null)
            {
                Debug.Write(contact.Username);
                return RedirectToAction("Create", "Contact");
            }
            else if (contact == null)
            {
                Debug.Write("This shouldnt be here");
            }

            return View(contact);
        }

        // GET: Contacts/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Contacts/Create
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

                if (_context.Contacts.Where(u => u.Username == model.Username).Any())
                {
                    ViewBag.Message = "Username Already Taken";
                    return View(model);
                }

            //Need to find out why its not being valid
            _context.Contacts.Add(newContact);
            _context.SaveChanges();
            return RedirectToAction("Details");

        }

        // GET: Contacts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Contact contact = _context.Contacts.Find(id);

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
                ZipCode = contact.ZipCode
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

            string email = User.Identity.GetUserName();

            Contact existing = _context.Contacts.FirstOrDefault(c => c.Email == email);

            existing.FirstName = model.FirstName;
            existing.LastName = model.LastName;
            existing.PhoneNumber = model.PhoneNumber;
            existing.Address1 = model.Address1;
            existing.Address2 = model.Address2;
            existing.City = model.City;
            existing.State = model.State;
            existing.ZipCode = model.ZipCode;

            _context.Entry(existing).State = EntityState.Modified;
            _context.SaveChanges();

            return RedirectToAction("Details");
        }

        // POST: Contacts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(EditContactViewModel model)
        {
            return RedirectToAction("RemoveAccount", "Account", new { id = model.ContactId});
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
