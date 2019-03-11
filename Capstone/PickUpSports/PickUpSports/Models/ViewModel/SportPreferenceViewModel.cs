using PickUpSports.Models.DatabaseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PickUpSports.Models.ViewModel
{
    public class SportPreferenceViewModel
    {
        public int ContactID { get; set; }
        public string ContactUsername { get; set; }
        public string SportName { get; set; }
        public int SportID { get; set; }
        public List<SportPreference> SportPreferences { get; set; }
    }
}