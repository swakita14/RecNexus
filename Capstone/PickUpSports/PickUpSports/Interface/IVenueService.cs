using System;
using System.Collections.Generic;
using PickUpSports.Models.DatabaseModels;

namespace PickUpSports.Interface
{
   public interface IVenueService
   {
       List<Venue> GetAllVenues();

       Venue GetVenueById(int venueId);

       void EditVenue(Venue venue);

       Location GetVenueLocation(int venueId);

       List<Review> GetVenueReviews(int venueId);

       List<BusinessHours> GetVenueBusinessHours(int venueId);

       List<BusinessHours> GetAllBusinessHours();

       bool VenueHasOwner(Venue venue);

       bool IsVenueAvailable(List<BusinessHours> venueHours, DateTime startDateTime, DateTime endDateTime);

       void UpdateVenues();
        
       double CalculateVenueDistance(double lat1, double long1, double lat2, double long2);

       string GetVenueNameById(int venueId);

   }
}
