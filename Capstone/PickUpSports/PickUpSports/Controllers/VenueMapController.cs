using PickUpSports.Interface;
using PickUpSports.Models.DatabaseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace PickUpSports.Controllers
{
    public class VenueMapController : ApiController
    { 

        private IVenueServices _venuesServices;

        public VenueMapController(IVenueServices venueService)
        {
            _venuesServices = venueService;
        }

        public IEnumerable<Venue> Get()
        {
            return _venuesServices.Get();
        }

        public Venue Get(int id)
        {
            return _venuesServices.Get().Where(v => v.VenueId == id).FirstOrDefault();
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