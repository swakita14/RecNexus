﻿using System.Collections.Generic;
using PickUpSports.Models.ViewModel.GameController;

namespace PickUpSports.Models.ViewModel.ContactController
{
    /**
     * Model for properties that will show on user's public profile
     */
    public class ProfileViewModel
    {
        public string Username { get; set; }

        public SportPreferenceViewModel  SportPreferences { get; set; }

        public TimePreferenceListViewModel TimePreferences { get; set; }

        public List<GameListViewModel> Games { get; set; }

    }
}