using System;
using System.Linq;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using PickUpSports.DAL;
using PickUpSports.Models.DatabaseModels;
using PickUpSports.Models.ViewModel;

namespace PickUpSports.Controllers
{
    public class ReviewController : Controller
    {
        private readonly PickUpContext _context;

        public ReviewController(PickUpContext context)
        {
            _context = context;
        }

        public ActionResult Create(int id)
        {
            // Confirm user is logged in (visitors can't leave reviews)
            string email = User.Identity.GetUserName();
            Contact contact = _context.Contacts.FirstOrDefault(c => c.Email == email);

            if (contact == null) 
            {
                ModelState.AddModelError("NoContact", "Please login or register to leave a review.");
                return View();
            }

            // Get name of venue to display on View and return model
            Venue venue = _context.Venues.Find(id);

            var model = new CreateReviewViewModel
            {
                VenueId = venue.VenueId,
                VenueName = venue.Name
            };

            return View(model);
        }

        [HttpPost]
        public ActionResult Create(CreateReviewViewModel model)
        {
            // Get contact to tie to review
            string email = User.Identity.GetUserName();
            Contact contact = _context.Contacts.FirstOrDefault(c => c.Email == email);

            // Create and add Review to database
            Review review = new Review
            {
                IsGoogleReview = false,
                Timestamp = DateTime.Now,
                Comments = model.Comments,
                Rating = model.Rating,
                VenueId = model.VenueId,
                ContactId = contact.ContactId
            };

            _context.Reviews.Add(review);
            _context.SaveChanges();

            // Redirect user to Venue details page which shows all reviews
            return RedirectToAction("Details", "Venue", new {id = model.VenueId});
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