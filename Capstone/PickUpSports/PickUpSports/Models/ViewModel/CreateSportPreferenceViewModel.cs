﻿using PickUpSports.Models.DatabaseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PickUpSports.Models.ViewModel
{
    public class CreateSportPreferenceViewModel
    {
        public int ContactID { get; set; }
        public string ContactUsername { get; set; }
        public List<Sport> Sports { get; set; }
        //public string SportName { get; set; }
        public int SportID { get; set; }
    }
}