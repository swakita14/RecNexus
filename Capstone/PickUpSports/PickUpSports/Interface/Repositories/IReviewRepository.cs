using System.Collections.Generic;
using PickUpSports.Models.DatabaseModels;

namespace PickUpSports.Interface.Repositories
{
    public interface IReviewRepository
    {
        void EditReview(Review review);

        List<Review> GetReviewsByContactId(int contactId);
    }
}