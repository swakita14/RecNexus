using Newtonsoft.Json;

namespace PickUpSports.Data.GoogleAPI.Models
{
    public class AltId
    {
        [JsonProperty("place_id")]
        public string PlaceId { get; set; }

        [JsonProperty("scope")]
        public string Scope { get; set; }
    }
}
