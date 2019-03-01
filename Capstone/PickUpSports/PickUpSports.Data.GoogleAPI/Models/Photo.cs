using System.Collections.Generic;
using Newtonsoft.Json;

namespace PickUpSports.Data.GoogleAPI.Models
{
    public class Photo
    {
        [JsonProperty("height")]
        public int Height { get; set; }

        [JsonProperty("html_attributions")]
        public List<object> HtmlAttributions { get; set; }

        [JsonProperty("photo_reference")]
        public string PhotoReference { get; set; }

        [JsonProperty("width")]
        public int Width { get; set; }
    }
}
