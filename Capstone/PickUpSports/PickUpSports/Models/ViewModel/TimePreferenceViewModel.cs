using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PickUpSports.Models.ViewModel
{
    public enum DayOfWeek
    {
        Sunday = 1,
        Monday,
        Tuesday,
        Wednesday,
        Thurday,
        Friday,
        Saturday
    }

    public class TimePreferenceViewModel
    {
        public int TimePrefID { get; set; }

        public DayOfWeek DayOfWeek { get; set; }

        public DateTime? BeginTime { get; set; }

        public DateTime? EndTime { get; set; }

        public int ContactID { get; set; }
    }
}