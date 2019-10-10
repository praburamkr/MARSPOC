using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Notification
{
    public class Notification
    {
        public Notification(List<string> userids, string eventId, List<Dictionary<string, string>> payload)
        {
            this.UserIDs = userids;
            this.EventId = eventId;
            this.Data = payload;
        }

        //Tags is User Name
        public List<string> UserIDs { get; set; } //User Name List, could be email IDs or Unique IDs which were used for device registration
        public string EventId { get; set; } //APPOINTMENT or CHECKIN
        public List<Dictionary<string, string>> Data { get; set; } //Dynamicn Key Pair Value List 
    }

    public class User
    {
        public string UserId { get; set; }
        public string ClientType { get; set; }
        public string Status { get; set; }
    }

}



