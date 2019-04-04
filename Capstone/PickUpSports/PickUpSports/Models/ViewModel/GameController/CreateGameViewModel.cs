using System.ComponentModel;

namespace PickUpSports.Models.ViewModel.GameController
{
    public class CreateGameViewModel
    {
        [DisplayName("Pick a start and finish time:")]
        public string DateRange { get; set; }

        [DisplayName("Venue:")]
        public int VenueId { get; set; }

        [DisplayName("Sport:")]
        public int SportId { get; set; }
    }
}