namespace PickUpSports.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Contact
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ContactId { get; set; }

        [Required]
        [StringLength(128)]
        public string Username { get; set; }

        [Required]
        [StringLength(128)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(128)]
        public string LastName { get; set; }

        [Required]
        [StringLength(256)]
        public string Email { get; set; }

        [Required]
        [StringLength(128)]
        public string PhoneNumber { get; set; }

        [Required]
        [StringLength(256)]
        public string Address1 { get; set; }

        [Required]
        [StringLength(256)]
        public string Address2 { get; set; }

        [Required]
        [StringLength(256)]
        public string City { get; set; }

        [Required]
        [StringLength(256)]
        public string State { get; set; }

        [Required]
        [StringLength(256)]
        public string ZipCode { get; set; }
    }
}
