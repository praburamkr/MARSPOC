using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Appointment.Models
{
    public class NotificationPayload
    {
        [JsonProperty("appointment_id")]
        public int AppointmentId { get; set; }

        [JsonProperty("appointment_sender")]
        public string SenderEmail { get; set; }

        [JsonProperty("appointment_sendername")]
        public string SenderName { get; set; }

        [JsonProperty("appointment_message")]       
        public string Message { get; set; }

        public NotificationPayload()
        {
            this.AppointmentId = 0;
            this.SenderEmail = null;
            this.SenderName = null;
            this.Message = null;
        }

        public NotificationPayload(int Id)
        {
            this.AppointmentId = Id;
            this.SenderEmail = null;
            this.SenderName = null;
            this.Message = null;
        }
        
    }
}
