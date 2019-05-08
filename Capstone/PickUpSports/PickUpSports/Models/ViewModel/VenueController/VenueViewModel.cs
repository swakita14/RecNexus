using System.Collections.Generic;
using PickUpSports.Models.DatabaseModels;
using PickUpSports.Models.ViewModel.GameController;

namespace PickUpSports.Models.ViewModel.VenueController
{
    public class VenueViewModel
    {
        public int VenueId { get; set; }

        public string Name { get; set; }

        public string Phone { get; set; }

        public string Address1 { get; set; }

        public string Address2 { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public string ZipCode { get; set; }

        public List<BusinessHoursViewModel> BusinessHours { get; set; }

        public decimal? AverageRating { get; set; }

        public string LatitudeCoord { get; set; }

        public string LongitudeCoord { get; set; }

        public double Distance { get; set; }

        public bool HasVenueOwner { get; set; }
    }
}