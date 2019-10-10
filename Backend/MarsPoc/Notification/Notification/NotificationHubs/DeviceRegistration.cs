using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NotificationWebApi.NotificationHubs
{
    public class DeviceRegistration
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public MobilePlatform Platform { get; set; }
        public string RegistrationId { get; set; }
        public string Handle { get; set; }
        public List<string> UserNames { get; set; } //Will Hold the User Name
    }
}
