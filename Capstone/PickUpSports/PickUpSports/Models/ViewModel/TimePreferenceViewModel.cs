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

    public class TimePreference
    {

        public int TimePrefID { get; set; }

        public DayOfWeek DayOfWeek { get; set; }

        public TimeSpan? BeginTime { get; set; }

        public TimeSpan? EndTime { get; set; }

        public int ContactID { get; set; }

    }
}