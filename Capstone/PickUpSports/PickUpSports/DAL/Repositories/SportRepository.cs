using PickUpSports.Interface.Repositories;
using PickUpSports.Models.DatabaseModels;

namespace PickUpSports.DAL.Repositories
{
    public class SportRepository : ISportRepository
    {
        private readonly PickUpContext _context;

        public SportRepository(PickUpContext context)
        {
            _context = context;
        }

        public Sport GetSportById(int id)
        {
            var result = _context.Sports.Find(id);
            return result;
        }
    }
}