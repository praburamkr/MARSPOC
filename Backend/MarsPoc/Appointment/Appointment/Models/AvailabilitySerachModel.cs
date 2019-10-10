using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Appointment.Models
{
    public class AvailabilitySerachModel
    {
        [JsonProperty("appt_type_id")]
        public int TypeId { get; set; }

        [JsonProperty("appt_sub_type_id")]
        public int SubTypeId { get; set; }

        [JsonProperty("client_id")]
        public int ClientId { get; set; }

        [JsonProperty("patient_id")]
        public int PatientId { get; set; }

        [JsonProperty("appt_date")]
        public DateTime AppointmentDate { get; set; }

        [JsonProperty("default_slot")]
        public Boolean DefaultSlot { get; set; }

        [JsonProperty("client_req_time")]
        public double ClientReqTime { get; set; }
    }
}
