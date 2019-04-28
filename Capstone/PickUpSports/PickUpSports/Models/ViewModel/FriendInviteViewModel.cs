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
        public List<ViewFriendsViewModel> Friends { get; set; }
        public List<GameListViewModel> Games { get; set; }
    }
}