using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using PickUpSports.Interface.Repositories;
using PickUpSports.Models.DatabaseModels;

namespace PickUpSports.DAL.Repositories
{
    public class ReviewRepository : IReviewRepository
    {
        private readonly PickUpContext _context;

        public ReviewRepository(PickUpContext context)
        {
            _context = context;
        }

        public Review AddReview(Review review)
        {
            _context.Reviews.Add(review);
            _context.SaveChanges();
            return review;
        }

        public void EditReview(Review review)
        {
            _context.Entry(review).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public List<Review> GetReviewsByContactId(int contactId)
        {
            List<Review> reviews = _context.Reviews.Where(s => s.ContactId == contactId).ToList();
            return reviews;
        }
    }
}