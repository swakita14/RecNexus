using System.Threading.Tasks;
using PickUpSports.Models.GoogleApiModels;

namespace PickUpSports.Interface
{
    public interface IPlacesApiClient
    {
        Task<PlaceSearchResponse> GetPlaces();

        Task<PlaceDetailsResponse> GetPlaceDetailsById(string placeId);
    }
}
