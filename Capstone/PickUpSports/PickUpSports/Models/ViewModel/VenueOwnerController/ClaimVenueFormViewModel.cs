using System.ComponentModel.DataAnnotations;
using System.Web;

namespace PickUpSports.Models.ViewModel.VenueOwnerController
{
    public class ClaimVenueFormViewModel
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string PhoneNumber { get; set; }

        public string CompanyName { get; set; }

        public int VenueId { get; set; }

        public string VenueName { get; set; }

        [Required]
        public HttpPostedFile Document { get; set; }
    }
}