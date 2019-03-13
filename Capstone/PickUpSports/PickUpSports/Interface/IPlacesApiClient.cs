using System.Collections.Generic;
using System.Threading.Tasks;
using PickUpSports.Models.GoogleApiModels;

namespace PickUpSports.Interface
{
    public interface IPlacesApiClient
    {
        Task<List<PlaceSearchResult>> GetVenues();

        Task<PlaceDetailsResponse> GetPlaceDetailsById(string placeId);
    }
}
