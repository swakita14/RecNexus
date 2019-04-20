using System.Collections.Generic;
using PickUpSports.Models.DatabaseModels;

namespace PickUpSports.Interface.Repositories
{
    public interface ITimePreferenceRepository
    {
        List<TimePreference> GetUsersTimePreferencesByContactId(int contactId);

        void Delete(TimePreference timePreference);
    }
}