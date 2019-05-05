<<<<<<< HEAD
﻿using PickUpSports.Models.DatabaseModels;
=======
﻿using System.Collections.Generic;
using PickUpSports.Models.DatabaseModels;
>>>>>>> upstream/development

namespace PickUpSports.Interface.Repositories
{
    public interface IVenueOwnerRepository
    {
        VenueOwner GetVenueOwnerById(int venueOwnerId);
        
        VenueOwner GetVenueOwnerByEmail(string email);

        VenueOwner GetVenueOwnerByVenueId(int venueId);

        VenueOwner AddVenueOwner(VenueOwner owner);

        void Edit(VenueOwner venueOwner);

        void Delete(VenueOwner venueOwner);

        List<VenueOwner> GetAllVenueOwners();
    }
}