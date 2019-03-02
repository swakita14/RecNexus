using System;
using DayOfWeek = PickUpSports.Models.Enums.DayOfWeek;

namespace PickUpSports.Models.ViewModel
{
    public class TimePreference
    {

        public int TimePrefID { get; set; }

        public DayOfWeek DayOfWeek { get; set; }

        public TimeSpan? BeginTime { get; set; }

        public TimeSpan? EndTime { get; set; }

        public int ContactID { get; set; }

    }
}