using PickUpSports.Interface.Repositories;
using PickUpSports.Models.DatabaseModels;

namespace PickUpSports.DAL.Repositories
{
    public class LocationRepository : ILocationRepository
    {
        private readonly PickUpContext _context;

        public LocationRepository(PickUpContext context)
        {
            _context = context;
        }

        public Location AddLocation(Location location)
        {
            _context.Locations.Add(location);
            _context.SaveChanges();
            return location;
        }
    }
}