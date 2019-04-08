using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PickUpSports.Models.ViewModel
{
    public class GameBySportViewModel
    {
        public List<SelectListItem> Sports { get; set; }
        public int? SportId { get; set; }
        public List<int> GameId { get; set; }
        public List<string> VenueName { get; set; }
        public List<DateTime> StartTime { get; set; }
        public List<DateTime> EndTime { get; set; }
        public List<string> ContactName { get; set; }
        public List<string> GameStatus { get; set; }
    }
}