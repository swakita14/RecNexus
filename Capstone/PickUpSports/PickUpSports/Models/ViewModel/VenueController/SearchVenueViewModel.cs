using System.Collections.Generic;

namespace PickUpSports.Models.ViewModel.VenueController
{
    public class SearchVenueViewModel
    {
        public string Search { get; set; }

        public string Filter { get; set; }

        public string CurrentLatitude { get; set; }

        public string CurrentLongitude { get; set; }

        public string Day { get; set; }

        public string Time { get; set; }

        public List<VenueViewModel> Venues { get; set; }
    }
}