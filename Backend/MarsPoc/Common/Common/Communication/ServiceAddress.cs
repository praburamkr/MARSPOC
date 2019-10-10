using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Communication
{
    public static class ServiceAddress
    {
        public const string ClientSearchUrl = "https://mars-petcare.eastus.cloudapp.azure.com/api/clients/";
        public const string ResourceSearchUrl = "https://mars-petcare.eastus.cloudapp.azure.com/api/resources/";

        public const string AppointmentsBaseUrl = "https://mars-petcare.eastus.cloudapp.azure.com/api/appointments/";
        public const string ClientsBaseUrl = "https://mars-petcare.eastus.cloudapp.azure.com/api/clients/";   //52.179.122.246
        public const string PatientsBaseUrl = "https://mars-petcare.eastus.cloudapp.azure.com/api/patients/"; 
        public const string ResourcesBaseUrl = "https://mars-petcare.eastus.cloudapp.azure.com/api/resources/"; //52.224.64.167
        public const string AvailabilityBaseUrl = "https://mars-petcare.eastus.cloudapp.azure.com/api/resource/availability/";

        public const string ClientSearchAllUrl = "https://mars-petcare.eastus.cloudapp.azure.com/api/clients/searchall";
        public const string ResourceSearchAllUrl = "https://mars-petcare.eastus.cloudapp.azure.com/api/resources/searchall";

        public const string NotificationsAllUrl = "https://mars-petcare.eastus.cloudapp.azure.com/api/notifications/sendall";
        public const string NotificationsUrl = "https://mars-petcare.eastus.cloudapp.azure.com/api/notifications/send";
        public const string EmailNotificationUrl = "https://mars-petcare.eastus.cloudapp.azure.com/api/notifications/email/send";     
    }
}
