namespace PickUpSports.Interface
{
   public interface IVenueService
   {
       void UpdateVenues();

       double CalculateVenueDistance(double lat1, double long1, double lat2, double long2);
   }
}
