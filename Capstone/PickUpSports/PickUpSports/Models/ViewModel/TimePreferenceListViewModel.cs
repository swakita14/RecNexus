using System.Collections.Generic;

namespace PickUpSports.Models.ViewModel
{
    public class TimePreferenceListViewModel
    {
        public int ContactId { get; set; }

        public List<TimePreferenceViewModel> TimePreferences { get; set; }
    }
}