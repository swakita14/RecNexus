using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PickUpSports.Models.ViewModel.GameController
{
    public class PickUpGameViewModel
    {
        public int GameId { get; set; }

        public int PickUpGameId { get; set; }

        public string Username { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }
    }
}