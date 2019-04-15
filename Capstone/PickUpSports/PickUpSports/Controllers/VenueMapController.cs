using PickUpSports.Models.DatabaseModels;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using PickUpSports.DAL;

namespace PickUpSports.Controllers
{
    public class VenueMapController : ApiController
    {
        private readonly PickUpContext _context;

        public VenueMapController(PickUpContext context)
        {
            _context = context;
        }


        public IEnumerable<Venue> Get()
        {
            return _context.Venues.ToList();
        }

        public Venue Get(int id)
        {
            return _context.Venues.Find(id);
        }
        // POST: api/Events
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Events/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Events/5
        public void Delete(int id)
        {
        }

    }
}