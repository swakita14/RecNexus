using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using PickUpSports.DAL;
using PickUpSports.Models.DatabaseModels;
using PickUpSports.Models.ViewModel;

namespace PickUpSports.Controllers
{
    [Authorize]
    public class TimePreferencesController : Controller
    {
        private PickUpContext _context = new PickUpContext();

        // GET: TimePreferences
        public ActionResult Index()
        {
            return View(_context.TimePreferences.ToList());
        }

        // GET: TimePreferences/Details/5
        public ActionResult Details(int? id)
        {
            //Identify the person using email
            string newContactEmail = User.Identity.GetUserName();

            //find the contact
            Contact contact = _context.Contacts.FirstOrDefault(x => x.Email == newContactEmail);

            //match the contactId w/ the time preference using ID
            TimePreference timePreference = _context.TimePreferences.FirstOrDefault(x => x.ContactID == contact.ContactId);

            if (timePreference == null)
            {
                return HttpNotFound();
            }
            return View(timePreference);
        }

        // GET: TimePreferences/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TimePreferences/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CreateTimePreferenceViewModel model)
        {
            //Check the model state
            if (!ModelState.IsValid) return View(model);

            //Find the user to add the ID with
            string email = User.Identity.GetUserName();
            Contact contact = _context.Contacts.FirstOrDefault(c => c.Email == email);

            //Creating the TimePreference tieing in with contact using contact credentials (ContactID)
            TimePreference timePreference = new TimePreference()
            {
                ContactID = contact.ContactId,
                BeginTime = model.BeginTime,
                DayOfWeek = (int) model.DayOfWeek,
                EndTime = model.EndTime,
            };

            //Save to dB
            _context.TimePreferences.Add(timePreference);
            _context.SaveChanges();
            
             return RedirectToAction("Index", "TimePreferences");

        }

        // GET: TimePreferences/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TimePreference timePreference = _context.TimePreferences.Find(id);
            if (timePreference == null)
            {
                return HttpNotFound();
            }
            return View(timePreference);
        }

        // POST: TimePreferences/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TimePrefID,DayOfWeek,BeginTime,EndTime,ContactID")] TimePreference timePreference)
        {
            if (ModelState.IsValid)
            {
                _context.Entry(timePreference).State = EntityState.Modified;
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(timePreference);
        }

        // GET: TimePreferences/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TimePreference timePreference = _context.TimePreferences.Find(id);
            if (timePreference == null)
            {
                return HttpNotFound();
            }
            return View(timePreference);
        }

        // POST: TimePreferences/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TimePreference timePreference = _context.TimePreferences.Find(id);
            _context.TimePreferences.Remove(timePreference);
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
