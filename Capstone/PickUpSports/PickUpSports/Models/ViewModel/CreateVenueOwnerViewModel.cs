using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Org.BouncyCastle.Bcpg.OpenPgp;

namespace PickUpSports.Models.ViewModel
{
    public class CreateVenueOwnerViewModel
    {
        public int VenueOwnerId { get; set; }

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

        public DateTime SignUpDate { get; set; }

        [Display(Name = "Venue:")]
        public string VenueName { get; set; }

        
    }
}