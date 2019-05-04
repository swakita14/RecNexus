using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using PickUpSports.Interface.Repositories;
using PickUpSports.Models.DatabaseModels;

namespace PickUpSports.DAL.Repositories
{
    public class VenueOwnerRepository : IVenueOwnerRepository
    {

        private readonly PickUpContext _context;

        public VenueOwnerRepository(PickUpContext context)
        {
            _context = context;
        }

        public VenueOwner GetVenueOwnerById(int venueOwnerId)
        {
            VenueOwner owner = _context.VenueOwners.Find(venueOwnerId);

            if (owner == null) return null;

            return owner;
        }

        public VenueOwner GetVenueOwnerByEmail(string email)
        {
            VenueOwner owner = _context.VenueOwners.FirstOrDefault(x => x.Email == email);

            if (owner == null) return null;

            return owner;
        }

        public VenueOwner GetVenueOwnerByVenueId(int venueId)
        {
            VenueOwner owner = _context.VenueOwners.FirstOrDefault(x => x.VenueId == venueId);

            if (owner == null) return null;

            return owner;
        }

        public VenueOwner AddVenueOwner(VenueOwner owner)
        {
            _context.VenueOwners.Add(owner);
            _context.SaveChanges();

            return owner;
        }

        public void Edit(VenueOwner owner)
        {
            _context.Entry(owner).State = EntityState.Modified;
            _context.SaveChanges();
        }
    }
}