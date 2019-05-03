using System.Collections.Generic;
using PickUpSports.Models.DatabaseModels;

namespace PickUpSports.Interface
{
   public interface IVenueService
   {
       List<Venue> GetAllVenues();

       Venue GetVenueById(int venueId);

       Location GetVenueLocation(int venueId);

       List<Review> GetVenueReviews(int venueId);

       List<BusinessHours> GetVenueBusinessHours(int venueId);

       List<BusinessHours> GetAllBusinessHours();

       void UpdateVenues();

       double CalculateVenueDistance(double lat1, double long1, double lat2, double long2);

       string GetVenueNameById(int venueId);
   }
}
