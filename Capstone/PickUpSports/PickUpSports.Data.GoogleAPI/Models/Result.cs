using System.Collections.Generic;
using Newtonsoft.Json;

namespace PickUpSports.Data.GoogleAPI.Models
{
    public class Result
    {
        [JsonProperty("geometry")]
        public Geometry Geometry { get; set; }

        [JsonProperty("icon")]
        public string Icon { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("opening_hours")]
        public OpeningHours OpeningHours { get; set; }

        [JsonProperty("photos")]
        public List<Photo> Photos { get; set; }

        [JsonProperty("place_id")]
        public string PlaceId { get; set; }

        [JsonProperty("scope")]
        public string Scope { get; set; }

        [JsonProperty("alt_ids")]
        public List<AltId> AltIds { get; set; }

        [JsonProperty("reference")]
        public string Reference { get; set; }

        [JsonProperty("types")]
        public List<string> Types { get; set; }

        [JsonProperty("vicinity")]
        public string Vicinity { get; set; }
    }
}
