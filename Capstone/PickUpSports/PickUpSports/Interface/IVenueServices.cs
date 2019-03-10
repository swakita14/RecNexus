using PickUpSports.Models.DatabaseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PickUpSports.Interface
{
   public interface IVenueServices
    {
        IQueryable<Venue> Get();
    }
}
