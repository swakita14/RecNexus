using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PickUpSports.Models.ViewModel.VenueOwnerController
{
    public class MessageOwnerViewModel
    {
        public int VenueOwnerId { get; set; }

        public string Email { get; set; }

        public string Subject { get; set; }

        public string BodyMessage { get; set; }
    }
}