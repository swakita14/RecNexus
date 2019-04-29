using PickUpSports.Models.DatabaseModels;

namespace PickUpSports.Interface.Repositories
{
    public interface ILocationRepository
    {
        Location AddLocation(Location location);
    }
}