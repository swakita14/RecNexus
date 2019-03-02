using System.Threading.Tasks;
using PickUpSports.Data.GoogleAPI.Models;

namespace PickUpSports.Data.GoogleAPI.Interfaces
{
    public interface IPlacesApiClient
    {
        Task<PlaceSearchResponse> GetPlaces();

        Task<PlaceDetailsResponse> GetPlaceDetailsById(string placeId);
    }
}
