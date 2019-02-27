﻿using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PickUpSports.Models.ViewModel
{
    public class CreateContactViewModel
    {
        public int ContactId { get; set; }

        [Required]
        [DisplayName("User Name:")]
        public string Username { get; set; }

        [Required]
        [DisplayName("First Name:")]
        public string FirstName { get; set; }

        [Required]
        [DisplayName("Last Name:")]
        public string LastName { get; set; }

        [Required]
        [DisplayName("Email:")]
        public string Email { get; set; }

        [Required]
        [DisplayName("Phone Number:")]
        public string PhoneNumber { get; set; }

        [Required]
        [DisplayName("Address 1:")]
        public string Address1 { get; set; }

        [DisplayName("Address 2 (Optional):")]
        public string Address2 { get; set; }

        [Required]
        [DisplayName("City:")]
        public string City { get; set; }

        [Required]
        [DisplayName("State:")]
        public string State { get; set; }

        [Required]
        [DisplayName("Zip Code:")]
        public string ZipCode { get; set; }

    }
}