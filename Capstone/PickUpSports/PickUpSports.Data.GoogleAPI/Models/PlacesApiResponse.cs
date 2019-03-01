using System.Collections.Generic;
using Newtonsoft.Json;

namespace PickUpSports.Data.GoogleAPI.Models
{
    public class PlacesApiResponse
    {
        [JsonProperty("html_attributions")]
        public List<object> HtmlAttributions { get; set; }

        [JsonProperty("next_page_token")]
        public string NextPageToken { get; set; }

        [JsonProperty("results")]
        public List<Result> Results { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }
    }
}
