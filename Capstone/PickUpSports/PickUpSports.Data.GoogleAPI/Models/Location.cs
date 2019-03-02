using Newtonsoft.Json;

namespace PickUpSports.Data.GoogleAPI.Models
{
    public class Location
    {
        [JsonProperty("lat")]
        public double Latitude { get; set; }

        [JsonProperty("lng")]
        public double Longitude { get; set; }
    }
}
