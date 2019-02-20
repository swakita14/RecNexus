using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PickUpSports.Models.ViewModel
{
    public class CreateContactViewModel
    {
        public int ContactId { get; set; }

       
        public string Username { get; set; }

        
        public string FirstName { get; set; }

       
        public string LastName { get; set; }

        [Required]
        public string Email { get; set; }

       
        public string PhoneNumber { get; set; }

      
        public string Address1 { get; set; }

       
        public string Address2 { get; set; }

      
        public string City { get; set; }

       
        public string State { get; set; }

       
        public string ZipCode { get; set; }

    }
}