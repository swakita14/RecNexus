using PickUpSports.Interface.Repositories;

namespace PickUpSports.DAL.Repositories
{
    public class SportRepository : ISportRepository
    {
        private readonly PickUpContext _context;

        public SportRepository(PickUpContext context)
        {
            _context = context;
        }

        public string GetSportNameById(int id)
        {
            var result = _context.Sports.Find(id);
            return result.SportName;
        }
    }
}