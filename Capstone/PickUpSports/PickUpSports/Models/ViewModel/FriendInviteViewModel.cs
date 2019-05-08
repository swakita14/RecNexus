﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using PickUpSports.Models.ViewModel.GameController;

namespace PickUpSports.Models.ViewModel
{
    public class FriendInviteViewModel
    {
        [Required]
        public int ContactId { get; set; }
        
        [Required]
        [DisplayName("Friend")]
        public int FriendId { get; set; }

        [Required]
        [DisplayName("Game")]
        public int GameId { get; set; }

     
    }
}