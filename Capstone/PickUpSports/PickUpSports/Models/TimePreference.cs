using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PickUpSports.Models
{
    public class TimePreference
    {

        public int TimePrefID { get; set; }

        public byte? DayOfWeek { get; set; }

        public TimeSpan? BeginTime { get; set; }

        public TimeSpan? EndTime { get; set; }

        public int ContactID { get; set; }

    }

}