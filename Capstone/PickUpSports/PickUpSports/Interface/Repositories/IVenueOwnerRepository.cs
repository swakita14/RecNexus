using System.Collections.Generic;
using PickUpSports.Models.DatabaseModels;

namespace PickUpSports.Interface.Repositories
{
    public interface IVenueOwnerRepository
    {
        VenueOwner AddVenueOwner(VenueOwner venueOwner);

        VenueOwner GetVenueOwnerById(int id);

        void Edit(VenueOwner venueOwner);

        void Delete(VenueOwner venueOwner);

        List<VenueOwner> GetAllVenueOwners();
    }
}