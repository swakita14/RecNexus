using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PickUpSports.Models.DatabaseModels;

namespace PickUpSports.Models.ViewModel
{
    public class DetailTimePreferenceViewModel
    {
        public Contact Contact { get; set; }

        public List<CreateTimePreferenceViewModel> DetailViewModels { get; set; }
    }
}