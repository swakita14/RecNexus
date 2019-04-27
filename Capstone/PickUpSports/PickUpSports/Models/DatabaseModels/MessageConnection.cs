namespace PickUpSports.Models.DatabaseModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("MessageConnection")]
    public partial class MessageConnection
    {
        [Key]
        public int ConnectionID { get; set; }

        public int ContactID { get; set; }

        public int MessageContactID { get; set; }

        public bool? Connected { get; set; }
    }
}
