using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using PickUpSports.Models.DatabaseModels;

namespace PickUpSports.Models.ViewModel.GameController
{
    public class ViewGameViewModel
    {
      
    
        public int GameId { get; set; }

        public int PickUpGameId { get; set; }

        public int ContactId { get; set; }

        [DisplayName("Venue:")]
        public string Venue { get; set; }

        [DisplayName("Sport:")]
        public string Sport { get; set; }

        [DisplayName("Status:")]
        public string Status { get; set; }

        [DisplayName("Start Date:")]
        public string StartDate { get; set; }

        [DisplayName("End Date:")]
        public string EndDate { get; set; }

        [DisplayName("Contact Name:")]
        public string ContactName { get; set; }
    }
}