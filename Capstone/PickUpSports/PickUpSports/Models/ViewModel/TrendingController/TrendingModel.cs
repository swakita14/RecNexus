using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace PickUpSports.Models.ViewModel
{
    public class TrendingViewModel
    {
        public int GameId { get; set; }
        public string SportName { get; set; }
        public string VenueName { get; set; }
        public int SportId { get; set; }
        public int VenueId { get; set; }
        public int Rating { get; set; }

        public override string ToString()
        {
            return GameId + "," + SportName + "," + VenueName + "," + SportId + "," + VenueId + "," + Rating;
        }
    }
}