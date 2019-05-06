using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PickUpSports.Models.ViewModel.GameController;

namespace PickUpSports.Models.ViewModel
{
    public class FriendInviteViewModel
    {
        public int ContactId { get; set; }

       public int FriendId { get; set; }
       public string FriendName { get; set; }

       public int GameId { get; set; }
       public int VenueId { get; set; }

     
    }
}