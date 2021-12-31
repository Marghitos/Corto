using Newtonsoft.Json;
using System;

namespace Corto.BL.Models
{

    public class UrlItem
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "shortned_url")]
        public string ShortenedUrl { get; set; }

        [JsonProperty(PropertyName = "original_url")]
        public string OriginalUrl { get; set; }

        [JsonProperty(PropertyName = "expiry_date")]
        public DateTime ExpiryDate { get; set; }
    }
}
