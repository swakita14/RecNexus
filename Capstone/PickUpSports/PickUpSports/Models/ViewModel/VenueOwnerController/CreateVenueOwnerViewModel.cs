using System.ComponentModel.DataAnnotations;

namespace PickUpSports.Models.ViewModel.VenueOwnerController
{
    public class CreateVenueOwnerViewModel
    {
        public int VenueId { get; set; }

        [Required]
        [Display(Name = "First Name:")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name:")]
        public string LastName { get; set; }

        [Required]
        [Display(Name = "Email:")]
        public string Email { get; set; }

        [Display(Name = "Phone Number:")]
        public string PhoneNumber { get; set; }

        [Display(Name = "Company Name:")]
        public string CompanyName { get; set; }

        [Required]
        [Display(Name = "Subject:")]
        public string MessageSubject { get; set; }

        [Required]
        [Display(Name = "Body:")]
        public string MessageBody { get; set; }

        public string TemporaryPassword { get; set; }
    }
}