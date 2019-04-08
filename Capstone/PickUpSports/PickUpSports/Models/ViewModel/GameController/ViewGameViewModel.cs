using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PickUpSports.Models.DatabaseModels;

namespace PickUpSports.Models.ViewModel.GameController
{
    public class ViewGameViewModel
    {
        public Contact ContactPerson { get; set; }

        public string Status { get; set; }

        public string StartTime { get; set; }

        public string EndTime { get; set; }
    }
}