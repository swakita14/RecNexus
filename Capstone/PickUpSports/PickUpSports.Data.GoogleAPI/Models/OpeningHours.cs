using Newtonsoft.Json;

namespace PickUpSports.Data.GoogleAPI.Models
{
    public class OpeningHours
    {
        [JsonProperty("open_now")]
        public bool OpenNow { get; set; }
    }
}
