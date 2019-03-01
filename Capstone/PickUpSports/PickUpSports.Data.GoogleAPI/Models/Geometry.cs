using Newtonsoft.Json;

namespace PickUpSports.Data.GoogleAPI.Models
{
    public class Geometry
    {
        [JsonProperty("location")]
        public Location Location { get; set; }
    }
}
