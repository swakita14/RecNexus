using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PickUpSports.Interface;
using PickUpSports.Interface.Repositories;
using PickUpSports.Models.DatabaseModels;

namespace PickUpSports.Services
{
    public class VenueOwnerServices : IVenueOwnerService
    {

        private readonly IVenueOwnerRepository _venueOwnerRepository;

        public VenueOwnerServices(IVenueOwnerRepository venueOwnerRepository)
        {
            _venueOwnerRepository = venueOwnerRepository;
        }

        public VenueOwner AddVenueOwner(VenueOwner owner)
        {
            return _venueOwnerRepository.AddVenueOwner(owner);
        }

        public void EditVenueOwner(VenueOwner owner)
        {
            _venueOwnerRepository.Edit(owner);
        }

        public VenueOwner GetVenueOwnerByEmail(string email)
        {
            return _venueOwnerRepository.GetVenueOwnerByEmail(email);
        }

        public List<VenueOwner> GetAllVenueOwners()
        {
            return _venueOwnerRepository.GetAllVenueOwners();
        }

        public VenueOwner GetVenueOwnerById(int venueOwnerId)
        {
            return _venueOwnerRepository.GetVenueOwnerById(venueOwnerId);
        }

        public VenueOwner GetVenueOwnerByVenueId(int venueId)
        {
            return _venueOwnerRepository.GetVenueOwnerByVenueId(venueId);
        }

        public bool IsVenueOwner(string email)
        {
            VenueOwner owner = _venueOwnerRepository.GetVenueOwnerByEmail(email);

            if (owner == null) return false;

            return true;
        }

        public bool VenueHasOwner(Venue venue)
        {
            var existingVenueOwner = _venueOwnerRepository.GetVenueOwnerByVenueId(venue.VenueId);

            if (existingVenueOwner == null) return false;

            return true;
        }


    }
}