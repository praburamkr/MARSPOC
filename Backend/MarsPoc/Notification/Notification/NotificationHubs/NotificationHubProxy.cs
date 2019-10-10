using GL.MARS.Models.NotificationEvents;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Azure.NotificationHubs;
using Microsoft.Azure.NotificationHubs.Messaging;
using Newtonsoft.Json;
using Notification.Interface;
using Notification.NotificationHubs;
using NotificationWebApi.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Notification.Repositories;

namespace NotificationWebApi.NotificationHubs
{
    public class NotificationHubProxy
    {
        private NotificationHubConfiguration _configuration;
        private NotificationHubClient _hubClient;
        private readonly UserRepository userContext;

        public NotificationHubProxy(NotificationHubConfiguration configuration, UserRepository userContext)
        {
            _configuration = configuration;
            _hubClient = NotificationHubClient.CreateClientFromConnectionString(_configuration.ConnectionString, _configuration.HubName);
            this.userContext = userContext;
        }

        /// 
        /// <summary>
        /// Get registration ID from Azure Notification Hub
        /// </summary>
        public async Task<string> CreateRegistrationId()
        {
            return await _hubClient.CreateRegistrationIdAsync();
        }

        /// 
        /// <summary>
        /// Delete registration ID from Azure Notification Hub
        /// </summary>
        /// <param name="registrationId"></param>
        public async Task<HubResponse> DeleteRegistration(string registrationId)
        {
            try
            {
                await _hubClient.DeleteRegistrationAsync(registrationId);
                return new HubResponse();
            }
            catch (Exception ex)
            {
                return new HubResponse().AddErrorMessage("Deletion of Registration failed!. PLease register once again. \n Exception Message:" + ex.Message.ToString());
            }
        }

        /// 
        /// <summary>
        /// Register device to receive push notifications. 
        /// Registration ID ontained from Azure Notification Hub has to be provided
        /// Then basing on platform (Android, iOS or Windows) specific
        /// handle (token) obtained from Push Notification Service has to be provided
        /// </summary>
        /// <param name="id"></param>
        /// <param name="deviceUpdate"></param>
        /// <returns></returns>
        /// 
        /// <summary>
        /// Register device to receive push notifications. 
        /// Registration ID ontained from Azure Notification Hub has to be provided
        /// Then basing on platform (Android, iOS or Windows) specific
        /// handle (token) obtained from Push Notification Service has to be provided
        /// </summary>
        /// <param name="id"></param>
        /// <param name="deviceUpdate"></param>
        /// <returns></returns>
        public async Task<HubResponse> RegisterForPushNotifications(DeviceRegistration deviceUpdate)
        {
            RegistrationDescription registrationDescription = null;

            switch (deviceUpdate.Platform)
            {
                case MobilePlatform.wns:
                    registrationDescription = new WindowsRegistrationDescription(deviceUpdate.Handle);
                    break;
                case MobilePlatform.apns:
                    registrationDescription = new AppleRegistrationDescription(deviceUpdate.Handle);
                    break;
                case MobilePlatform.gcm:
                    registrationDescription = new FcmRegistrationDescription(deviceUpdate.Handle);
                    break;
                default:
                    return new HubResponse().AddErrorMessage("Please provide correct platform notification service name.");
            }

            registrationDescription.RegistrationId = deviceUpdate.RegistrationId;
            if (deviceUpdate.UserNames != null)
                registrationDescription.Tags = new HashSet<string>(deviceUpdate.UserNames);

            try
            {
                await _hubClient.CreateOrUpdateRegistrationAsync(registrationDescription);
                return new HubResponse();
            }
            catch (MessagingException)
            {
                return new HubResponse().AddErrorMessage("Registration failed because of HttpStatusCode.Gone. PLease register once again.");
            }
            catch (Exception ex)
            {
                return new HubResponse().AddErrorMessage("Registration failed" + ex.Message);
            }
        }

        /// <summary>
        /// Send push notification to specific platform (Android, iOS or Windows)
        /// </summary>
        /// <param name="newNotification"></param>
        /// <returns></returns>
        public async Task<HubResponse<NotificationOutcome>> SendNotification(Common.Notification.Notification newNotification
            , IHubContext<WebNotificationHub, ITypedHubClient> _hubSignalRContext)
        {
            string webMessage, devMessage;
            try
            {
                NotificationOutcome outcome = null;

                if (newNotification == null || newNotification?.UserIDs == null || newNotification?.UserIDs?.Count == 0)
                {
                    foreach (Dictionary<string, string> itemData in newNotification.Data)
                    {
                        devMessage = "{ \"aps\":{ \"alert\":\"Mars-Notification\",\"sound\":\"default\",\"badge\":1},\"EventID\":\"" + newNotification.EventId + "\", \"UserID\": \"\",\"Data\":{" + buildNotificationData(itemData) + " } }";

                        //Broadcast Message Dev
                        outcome = await _hubClient.SendAppleNativeNotificationAsync(devMessage);

                        //Broadcast Message Web    
                        webMessage = JsonConvert.SerializeObject(new { EventID = newNotification.EventId, UserID = "", Data = itemData });
                        await _hubSignalRContext.Clients.All.BroadcastMessage(webMessage,"");
                    }
                }
                else
                {
                    IDictionary<string, string> objClientTypes = GetClientTypes(newNotification?.UserIDs);
                    if (objClientTypes != null)
                    {
                        foreach (KeyValuePair<string, string> item in objClientTypes)
                        {
                            foreach (Dictionary<string, string> itemData in newNotification.Data)
                            {
                                if (item.Value.ToUpper() == "WEB")
                                {
                                    webMessage = JsonConvert.SerializeObject(new { EventID = newNotification.EventId, UserID = item.Key, Data = itemData });
                                    await _hubSignalRContext.Clients.All.BroadcastMessage(webMessage, item.Key);
                                }
                                else
                                {
                                    devMessage = "{ \"aps\":{ \"alert\":\"Mars-Notification\",\"sound\":\"default\",\"badge\":1},\"EventID\":\"" + newNotification.EventId + "\", \"UserID\": \"" + item.Key + "\",\"Data\":{" + buildNotificationData(itemData) + " } }";
                                    outcome = await _hubClient.SendAppleNativeNotificationAsync(devMessage, item.Key); //Key is User ID
                                }
                            }
                        }
                    }
                }
                return new HubResponse<NotificationOutcome>();
            }
            catch (Exception ex)
            {
                return new HubResponse<NotificationOutcome>().SetAsFailureResponse().AddErrorMessage(ex.Message);
            }
        }

        private IDictionary<string, string> GetClientTypes(List<string> notificationUserList)
        {            
            try
            {
                return userContext.FilterSearchByUserList(notificationUserList);
            }
            catch (Exception ex)
            {
                return null;
            }            
        }

        /// 
        /// <summary>
        /// Send push notification to specific platform (Android, iOS or Windows)
        /// </summary>
        /// <param name="newNotification"></param>
        /// <returns></returns>
        public async Task<HubResponse<NotificationOutcome>> SendNotification(PushNotificationIntegrationEvent newNotification)
        {
            try
            {

                // For Apple iOS Push Notification; Other platform will be configured after PoC
                NotificationOutcome outcome = null;
                var alert = "{\"aps\":{\"alert\":\"" + newNotification + "\"}}";

                if (newNotification.Tags == null)
                    outcome = await _hubClient.SendAppleNativeNotificationAsync(alert);
                else
                    outcome = await _hubClient.SendAppleNativeNotificationAsync(alert, newNotification.Tags);

                if (outcome != null)
                {
                    if (!((outcome.State == NotificationOutcomeState.Abandoned) ||
                        (outcome.State == NotificationOutcomeState.Unknown)))
                        return new HubResponse<NotificationOutcome>();
                }

                return new HubResponse<NotificationOutcome>().SetAsFailureResponse().AddErrorMessage("Notification was not sent due to issue. Please send again.");
            }

            catch (MessagingException ex)
            {
                return new HubResponse<NotificationOutcome>().SetAsFailureResponse().AddErrorMessage(ex.Message);
            }

            catch (ArgumentException ex)
            {
                return new HubResponse<NotificationOutcome>().SetAsFailureResponse().AddErrorMessage(ex.Message);
            }

            catch (Exception ex)
            {
                return new HubResponse<NotificationOutcome>().SetAsFailureResponse().AddErrorMessage(ex.Message);
            }
        }

        private string buildNotificationData(Dictionary<string, string> dataCollection)
        {
            string buildData = "";

            foreach (var item in dataCollection)
            {
                if (buildData == "")
                    buildData = "\"" + item.Key + "\":\"" + item.Value + "\"";
                else
                    buildData += ",\"" + item.Key + "\":\"" + item.Value + "\"";
            }

            return buildData;
        }
    }

}
