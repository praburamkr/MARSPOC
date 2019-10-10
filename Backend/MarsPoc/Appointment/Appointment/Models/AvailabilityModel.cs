using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Appointment.Models
{
    public class AvailabilityModel
    {
        [JsonProperty("ResourceId")]
        public int DoctorId { get; set; }

        [JsonProperty("Name")]
        public string DoctorName { get; set; }
        
        [JsonProperty("Date")]
        public DateTime AppointmentDate { get; set; }

        [JsonProperty("StartTime")]
        public DateTime StartTime { get; set; }

        [JsonProperty("EndTime")]
        public DateTime EndTime { get; set; }

        [JsonProperty("Duration")]
        public int Duration { get; set; }

        //[JsonProperty("StartTime")]
        //public TimeSpan StartTimeSpan { get; set; }

        //[JsonProperty("EndTime")]
        //public TimeSpan EndTimeSpan { get; set; }

    }
}
