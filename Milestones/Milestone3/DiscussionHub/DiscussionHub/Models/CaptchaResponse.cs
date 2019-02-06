using System.Collections.Generic;
using Newtonsoft.Json;

namespace DiscussionHub.Models
{
    public class CaptchaResponse
    {
        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonProperty("error-codes")]
        public List<string> ErrorMessage{ get; set; }
    }

}