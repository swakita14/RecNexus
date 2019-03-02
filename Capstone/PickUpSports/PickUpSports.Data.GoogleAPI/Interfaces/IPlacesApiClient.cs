using System.Collections.Generic;
using System.Threading.Tasks;
using PickUpSports.Data.GoogleAPI.Models;

namespace PickUpSports.Data.GoogleAPI.Interfaces
{
    public interface IPlacesApiClient
    {
        Task<List<Result>> GetPlaces();
    }
}
