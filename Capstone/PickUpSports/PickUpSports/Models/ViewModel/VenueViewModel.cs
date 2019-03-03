using System.Collections.Generic;

namespace PickUpSports.Models.ViewModel
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

        public decimal? AverageRating{ get; set; }

        public List<ReviewViewModel> Reviews { get; set; }
    }
}