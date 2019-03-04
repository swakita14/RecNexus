using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PickUpSports.Data.GoogleAPI.Interfaces;
using PickUpSports.Data.GoogleAPI.Models;
using RestSharp;
using static System.String;

namespace PickUpSports.Data.GoogleAPI
{
    public class PlacesApiClient : IPlacesApiClient
    {
        private static IRestClient _client;
        private readonly string _apiKey;

        public PlacesApiClient(IRestClient client, string apiKey)
        {
            _client = client;
            _apiKey = apiKey;
        }

        /**
         * Method that makes call to Google Places API and
         * retrieves list of places given specific parameters
         */
        public Task<PlaceSearchResponse> GetPlaces()
        {
            // Initialize API request with URL and API key
            RestRequest request = new RestRequest("/nearbysearch/json?", Method.GET);
            request.AddQueryParameter("key", _apiKey);

            // Add parameters to API request, these will need to be changed
            request.AddQueryParameter("keyword", "basketball");
            request.AddQueryParameter("location", "44.9429, -123.0351");
            request.AddQueryParameter("radius", "10000");
            request.AddQueryParameter("type", "park");

            // Get respones from API using above request
            IRestResponse response = _client.Execute(request);

            // Convert JSON response to model and return response
            PlaceSearchResponse apiResponse = JsonConvert.DeserializeObject<PlaceSearchResponse>(response.Content);
            return Task.FromResult(apiResponse);
        }

        /**
         * Method that makes call to Google Places API and
         * retrieves details of a single place given a Google Place ID
         */
        public Task<PlaceDetailsResponse> GetPlaceDetailsById(string placeId)
        {
            // Initialize API request with URL and API key
            RestRequest request = new RestRequest("/details/json?", Method.GET);
            request.AddQueryParameter("key", _apiKey);
            request.AddQueryParameter("placeid", placeId);

            // Only pull name, business hours, reviews, and address data
            request.AddQueryParameter("fields", "name,opening_hours,reviews,address_component,formatted_phone_number");

            // Get responses from API using above request
            IRestResponse response = _client.Execute(request);

            // Convert JSON response to model and return response
            PlaceDetailsResponse apiResponse = JsonConvert.DeserializeObject<PlaceDetailsResponse>(response.Content);
            return Task.FromResult(apiResponse);
        }
    }
}
