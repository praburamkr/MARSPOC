using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Authentication.Models
{
    public class ResponseToken
    {
        [JsonIgnore]
        public string token_type { get; set; }
        [JsonIgnore]
        public string scope { get; set; }
        [JsonIgnore]
        public string expires_in { get; set; }
        [JsonIgnore]
        public string ext_expires_in { get; set; }
        [JsonIgnore]
        public string not_before { get; set; }
        [JsonIgnore]
        public string resource { get; set; }
        //[JsonProperty(PropertyName = "token")]
        public string access_token { get; set; }
        [JsonIgnore]
        public string refresh_token { get; set; }
    }
}
