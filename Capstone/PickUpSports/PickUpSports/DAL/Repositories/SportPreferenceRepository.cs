using System.Collections.Generic;
using System.Linq;
using PickUpSports.Interface.Repositories;
using PickUpSports.Models.DatabaseModels;

namespace PickUpSports.DAL.Repositories
{
    public class SportPreferenceRepository : ISportPreferenceRepository
    {
        private readonly PickUpContext _context;

        public SportPreferenceRepository(PickUpContext context)
        {
            _context = context;
        }

        public List<SportPreference> GetUsersSportPreferencesByContactId(int contactId)
        {
            var preferences = _context.SportPreferences.Where(x => x.ContactID == contactId).ToList();
            return preferences;
        }

        public void Delete(SportPreference sportPreference)
        {
            _context.SportPreferences.Remove(sportPreference);
            _context.SaveChanges();
        }
    }
}