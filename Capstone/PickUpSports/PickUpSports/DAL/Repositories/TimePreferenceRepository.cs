using System.Collections.Generic;
using System.Linq;
using PickUpSports.Interface.Repositories;
using PickUpSports.Models.DatabaseModels;

namespace PickUpSports.DAL.Repositories
{
    public class TimePreferenceRepository : ITimePreferenceRepository
    {
        private readonly PickUpContext _context;

        public TimePreferenceRepository(PickUpContext context)
        {
            _context = context;
        }

        public List<TimePreference> GetUsersTimePreferencesByContactId(int contactId)
        {
            var preferences = _context.TimePreferences.Where(x => x.ContactID == contactId).ToList();
            return preferences;
        }

        public void Delete(TimePreference timePreference)
        {
            _context.TimePreferences.Remove(timePreference);
            _context.SaveChanges();
        }
    }
}