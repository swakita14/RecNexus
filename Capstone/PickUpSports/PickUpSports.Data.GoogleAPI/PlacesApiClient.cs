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

        public async Task<List<Result>> GetPlaces()
        {
            // Initialize API request with URL and API key
            RestRequest request = new RestRequest("/nearbysearch/json?", Method.GET);
            request.AddQueryParameter("key", _apiKey);

            // Add parameters to API request, these will need to be changed
            request.AddQueryParameter("keyword", "basketball");
            request.AddQueryParameter("location", "44.9429, -123.0351");
            request.AddQueryParameter("radius", "20000");
            request.AddQueryParameter("type", "park");

            // Get respones from API using above request
            IRestResponse response = await _client.ExecuteGetTaskAsync(request);

            // List to return
            List<Result> placeResults = new List<Result>();

            // Convert JSON response to model and add to results lists 
            PlacesApiResponse apiResponse = JsonConvert.DeserializeObject<PlacesApiResponse>(response.Content);
            placeResults.AddRange(apiResponse.Results);

            // If next_page_token exists, add token to API request and call again

            bool hasNextToken = !IsNullOrEmpty(apiResponse.NextPageToken);
            while (hasNextToken)
            {
                request.AddQueryParameter("pagetoken", apiResponse.NextPageToken);
                response = await _client.ExecuteGetTaskAsync(request);
                apiResponse = JsonConvert.DeserializeObject<PlacesApiResponse>(response.Content);
                placeResults.AddRange(apiResponse.Results);

                hasNextToken = !IsNullOrEmpty(apiResponse.NextPageToken);
            }

            // Return list of place results
            return placeResults;
        }
    }
}
