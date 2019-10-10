using Appointment.Models;
using Common.Base;
using Common.Communication;
using Common.Interfaces;
using Common.Models;
using Common.Notification;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Helpers;

namespace Appointment.Repositories
{
    public class AppointmentRepository : RepositoryBase<AppointmentModel, AppointmentRepository>
    {
        private readonly ILogHandler logHandler;
        private readonly ModelEqualityComparer<AppointmentModel> modelEqualityComparer;

        public DbSet<AppointmentTypeModel> ApptTypeSet { get; set; }
        public DbSet<ReasonCodeModel> ReasonSet { get; set; }

        public AppointmentRepository(DbContextOptions<AppointmentRepository> options, ILogHandler logHandler) : base(options, logHandler)
        {
            this.logHandler = logHandler;

            modelEqualityComparer = new ModelEqualityComparer<AppointmentModel>();
        }

        protected override IEnumerable<AppointmentModel> FilterSearch(AppointmentModel item)
        {
            IEnumerable<AppointmentModel> appList = new List<AppointmentModel>();

            Func<AppointmentModel, bool> filterFunc = null;

            if (item.StartTime != default(DateTime) && item.EndTime != default(DateTime))
                filterFunc += m => m.StartTime >= item.StartTime && m.EndTime <= item.EndTime;

            if (item.DoctorId > 0)
                filterFunc += x => x.DoctorId == item.DoctorId;

            if (item.TypeId > 0)
                filterFunc += x => x.TypeId == item.TypeId;

            if (filterFunc != null)
                appList = this.ModelSet.Where(filterFunc).ToArray();

            return appList;
        }

        public async Task<MarsResponse> CreateAllAsync(object jsonObj, string token)
        {
            var appts = (JsonConvert.DeserializeObject(jsonObj.ToString()) as dynamic)["appointments"];
            if (appts == null)
                throw new Exception("Wrong Parameters");

            //var sendEmail = (JsonConvert.DeserializeObject(jsonObj.ToString()) as dynamic)["sendEmail"];
            //var emailAddress = (JsonConvert.DeserializeObject(jsonObj.ToString()) as dynamic)["emailAddress"];

            //bool shouldSendEmail = true;
            //string ccEmailAddress = "";

            //if (sendEmail != null)
            //    shouldSendEmail = Convert.ToBoolean(sendEmail);
            //if (emailAddress != null)
            //    ccEmailAddress = emailAddress.ToString();

            AppointmentModel[] appointmentList = JsonConvert.DeserializeObject<AppointmentModel[]>(appts.ToString());

            if (appointmentList == null || !appointmentList.Any())
                throw new Exception("No Values inside");

            var stopwatch = new Stopwatch();
            stopwatch.Start();

            foreach (var item in appointmentList)
                await this.ModelSet.AddAsync(item);
            await this.SaveChangesAsync();

            stopwatch.Stop();
            this.logHandler.LogMetric(this.GetType().ToString() + " CreateAllAsync", stopwatch.ElapsedMilliseconds);

            List<NotificationPayload> payload = new List<NotificationPayload>();

            foreach (var item in appointmentList)
            {
                payload.Add(new NotificationPayload(item.Id));
            }

            SendNotification(null, payload, token, NotificationEvent.NEW_APPT);

            var appt = appointmentList[0];
            await AddOtherValues(appt, token);

            if (appt.ClientId > 0 && appt.IsEmail)
            {
                string subject = "Your appointment has been scheduled";
                string body = AppointmentMailFormat.GetAppointmentMail(appt);                
                SendEmailNotification(null, appt.ClientId, subject, body, token, appt.Email != null ? appt.Email : "");
            }

            return MarsResponse.GetResponse(appointmentList);
        }

        public IEnumerable<AppointmentModel> FilterSearchByResourceIdAndDate(AppointmentModel item, DateTime date)
        {
            IEnumerable<AppointmentModel> appList = new List<AppointmentModel>();

            if (item.DoctorId > 0 && date != default(DateTime))
            {
                //appList = appList.Union(this.ModelSet.Where(m => m.DoctorId == item.DoctorId && (m.StartTime.Date == date || m.EndTime.Date == date)), modelEqualityComparer); // (m.EndTime >= item.StartTime && m.EndTime <= item.EndTime) || (m.StartTime >= item.StartTime && m.StartTime <= item.EndTime));
                appList = (from apt in this.ModelSet
                           where apt.DoctorId == item.DoctorId && (apt.StartTime.Date == date || apt.EndTime.Date == date)
                           select apt).ToArray();
            }

            return appList;
        }

        public async Task<MarsResponse> UpdateStatus(AppointmentModel item)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            var mod = await this.ModelSet.FirstOrDefaultAsync(c => c.Id == item.Id);
            if (mod != null)
            {
                if (item.Status != 0)
                    mod.Status = item.Status;
                if (item.WorkflowState != 0)
                    mod.WorkflowState = item.WorkflowState;

                await this.SaveChangesAsync();
            }

            stopwatch.Stop();
            this.logHandler.LogMetric(this.GetType().ToString() + " UpdateStatus", stopwatch.ElapsedMilliseconds);

            return MarsResponse.GetResponse(mod, mod == null ? HttpStatusCode.NotFound : HttpStatusCode.OK);
        }

        public async Task<MarsResponse> UpdateStatusWithNotification(AppointmentModel item, string eventCode, string token)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            var mod = await this.ModelSet.FirstOrDefaultAsync(c => c.Id == item.Id);
            if (mod != null)
            {
                if (item.Status != 0)
                    mod.Status = item.Status;
                if (item.WorkflowState != 0)
                    mod.WorkflowState = item.WorkflowState;

                await this.SaveChangesAsync();
            }

            stopwatch.Stop();
            this.logHandler.LogMetric(this.GetType().ToString() + " UpdateStatus", stopwatch.ElapsedMilliseconds);

            if (mod != null)
            {
                NotificationPayload payload = new NotificationPayload(mod.Id);

                string email = string.Empty;
                string name = string.Empty;

                if (mod.DoctorId > 0 &&
                    (eventCode.CompareTo(NotificationEvent.CHECKIN) == 0 ||
                    eventCode.CompareTo(NotificationEvent.RESPOND) == 0 ||
                    eventCode.CompareTo(NotificationEvent.CLAIM) == 0))
                {
                    var resourceResult = await InterServiceCommunication.GetAsync(ServiceAddress.ResourceSearchUrl + mod.DoctorId.ToString(), token);

                    if (resourceResult != null)
                    {
                        var data = (JsonConvert.DeserializeObject(resourceResult) as dynamic)?["data"];
                        if (data?["email"] != null)
                        {
                            email = data["email"];
                        }
                        if (data?["resource_name"] != null)
                        {
                            name = data["resource_name"];
                        }
                    }
                }

                if (eventCode.CompareTo(NotificationEvent.RESPOND) == 0)
                {
                    payload.SenderEmail = email;
                    payload.SenderName = name;
                    if (!string.IsNullOrEmpty(item.Response))
                        payload.Message = item.Response;
                    else
                        payload.Message = string.Empty;
                }
                else if (eventCode.CompareTo(NotificationEvent.CLAIM) == 0)
                {
                    payload.SenderEmail = email;
                }
                else if (eventCode.CompareTo(NotificationEvent.CHECKIN) == 0)
                {
                    payload.SenderEmail = email;
                }

                if (eventCode.CompareTo(NotificationEvent.CHECKIN) == 0 &&
                   !string.IsNullOrEmpty(email))
                {
                    List<string> objEmailList = new List<string>();
                    objEmailList.Add(email);

                    SendNotification(objEmailList, new List<NotificationPayload>() { payload }, token, NotificationEvent.CHECKIN_NOTIFY);
                }

                SendNotification(null, new List<NotificationPayload>() { payload }, token, eventCode);
            }

            return MarsResponse.GetResponse(mod, mod == null ? HttpStatusCode.NotFound : HttpStatusCode.OK);
        }



        public async Task<MarsResponse> SearchAppointmentsAsync(object jsonObj, string token)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            AppointmentModel item = JsonConvert.DeserializeObject<AppointmentModel>(jsonObj.ToString());

            IEnumerable<AppointmentModel> apptList = null;
            List<Func<AppointmentModel, bool>> filterList = new List<Func<AppointmentModel, bool>>();

            if (item.StartTime != default(DateTime) && item.EndTime != default(DateTime))
                filterList.Add(appt => appt.StartTime >= item.StartTime && appt.EndTime <= item.EndTime);

            if (item.DoctorId > 0)
                filterList.Add(x => x.DoctorId == item.DoctorId);

            if (item.TypeId > 0)
                filterList.Add(x => x.TypeId == item.TypeId);

            var doc_str = (JsonConvert.DeserializeObject(jsonObj.ToString()) as dynamic)?["doctor_ids"]?.ToString();
            if (doc_str != null)
            {
                int[] doc_ids = JsonConvert.DeserializeObject<int[]>(doc_str);
                if (doc_ids != null && doc_ids.Any())
                    filterList.Add(appt => doc_ids.Contains(appt.DoctorId));
            }

            var apptStat_str = (JsonConvert.DeserializeObject(jsonObj.ToString()) as dynamic)?["appt_statuses"]?.ToString();
            if (apptStat_str != null)
            {
                int[] apptStat_ids = JsonConvert.DeserializeObject<int[]>(apptStat_str);
                if (apptStat_ids != null && apptStat_ids.Any())
                    filterList.Add(appt => apptStat_ids.Contains(appt.Status));
            }

            if (item.Status > 0)
                filterList.Add(appt => appt.Status == item.Status);

            if (item.ClientId > 0)
                filterList.Add(appt => appt.ClientId == item.ClientId);

            if (filterList.Count > 0)
                apptList = this.ModelSet.Where(appt => filterList.All(filter => filter(appt))).ToArray();

            //if (apptList != null)
            //    foreach (var appt in apptList)
            //        await AddOtherValues(appt, token);

            apptList = await AddAllOtherValues(apptList, token);

            MarsResponse resp = MarsResponse.GetResponse(apptList);
            if (apptList == null)
                resp.Code = HttpStatusCode.NotAcceptable;
            else if (apptList.Count() == 0)
                resp.Code = HttpStatusCode.NotFound;

            stopwatch.Stop();
            this.logHandler.LogMetric(this.GetType().ToString() + " SearchAppointmentsAsync", stopwatch.ElapsedMilliseconds);

            return MarsResponse.GetResponse(apptList);
        }

        public async Task<MarsResponse> GetAppAsync(int id, string token)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            var appt = await base.GetModelAsync(id);

            await AddOtherValues(appt, token);

            stopwatch.Stop();
            this.logHandler.LogMetric(this.GetType().ToString() + " GetAppAsync", stopwatch.ElapsedMilliseconds);

            return MarsResponse.GetResponse(appt);
        }

        public async Task<IEnumerable<AppointmentModel>> AddAllOtherValues(IEnumerable<AppointmentModel> apptList, string token)
        {
            if (apptList == null || !apptList.Any())
                return null;

            HashSet<int> clientIdList = new HashSet<int>(), patientIdList = new HashSet<int>(), resourceIdList = new HashSet<int>();
            HashSet<int> apptTypeIdList = new HashSet<int>(), reasonIdList = new HashSet<int>();
            foreach (var appt in apptList)
            {
                appt.ClientName = "Unknown";
                appt.DoctorName = "Unknown";
                appt.PatientName = "Unknown";

                clientIdList.Add(appt.ClientId);
                patientIdList.Add(appt.PatientId);
                resourceIdList.Add(appt.DoctorId);
                apptTypeIdList.Add(appt.SubTypeId);
                reasonIdList.Add(appt.ReasonId);
            }

            List<Task> tasks = new List<Task>();

            Dictionary<int, AppointmentTypeModel> apptTypeMap = null;
            Dictionary<int, ReasonCodeModel> reasonMap = null;
            Dictionary<int, dynamic> clientsMap = new Dictionary<int, dynamic>();
            Dictionary<int, dynamic> patientsMap = new Dictionary<int, dynamic>();
            Dictionary<int, dynamic> doctorsMap = new Dictionary<int, dynamic>();

            tasks.Add(Task.Run(() =>
            {
                apptTypeMap = this.ApptTypeSet.ToDictionary(o => o.Id);
                reasonMap = this.ReasonSet.ToDictionary(o => o.Id);
            }));

            tasks.Add(Task.Run(async () =>
            {
                var clientResult = await InterServiceCommunication.PostAsync(ServiceAddress.ClientSearchUrl + "searchall", token, clientIdList);

                if (clientResult != null)
                {
                    var data = (JsonConvert.DeserializeObject(clientResult) as dynamic)?["data"];

                    if (data != null)
                    {
                        foreach (dynamic obj in data)
                            clientsMap.Add(Convert.ToInt32(obj["id"]), obj);
                    }
                }
            }));

            tasks.Add(Task.Run(async () =>
            {
                var patientResult = await InterServiceCommunication.PostAsync("http://52.168.38.130/api/patients/searchall", token, patientIdList);

                if (patientResult != null)
                {
                    var data = (JsonConvert.DeserializeObject(patientResult) as dynamic)?["data"];

                    if (data != null)
                    {
                        foreach (dynamic obj in data)
                            patientsMap.Add(Convert.ToInt32(obj["id"]), obj);
                    }
                }
            }));

            tasks.Add(Task.Run(async () =>
            {
                var doctorResult = await InterServiceCommunication.PostAsync(ServiceAddress.ResourceSearchUrl + "searchall", token, resourceIdList);

                if (doctorResult != null)
                {
                    var data = (JsonConvert.DeserializeObject(doctorResult) as dynamic)?["data"];

                    if (data != null)
                    {
                        foreach (dynamic obj in data)
                            doctorsMap.Add(Convert.ToInt32(obj["id"]), obj);
                    }
                }
            }));

            await Task.WhenAll(tasks);

            foreach (var appt in apptList)
            {
                appt.AppointmentType = apptTypeMap[appt.SubTypeId];
                appt.Reason = reasonMap[appt.ReasonId];

                if (clientsMap.ContainsKey(appt.ClientId))
                {
                    appt.ClientName = clientsMap[appt.ClientId]["name"];
                    appt.ClientImageUrl = clientsMap[appt.ClientId]["imageUrl"];
                }

                if (patientsMap.ContainsKey(appt.PatientId))
                {
                    appt.PatientName = patientsMap[appt.PatientId]["name"];
                    appt.PatientImageUrl = patientsMap[appt.PatientId]["imageUrl"];
                }

                if (doctorsMap.ContainsKey(appt.DoctorId))
                    appt.DoctorName = doctorsMap[appt.DoctorId]["resourceName"];
            }

            return apptList;
        }

        private async Task AddOtherValues(AppointmentModel model, string token)
        {
            if (model == null)
                return;

            model.AppointmentType = await this.ApptTypeSet.SingleOrDefaultAsync(apType => apType.Id == model.SubTypeId);
            model.Reason = await this.ReasonSet.SingleOrDefaultAsync(rsn => rsn.Id == model.ReasonId);

            string respString = await InterServiceCommunication.GetAsync(ServiceAddress.ClientSearchUrl + model.ClientId, token);

            if (respString != null)
            {
                var data = (JsonConvert.DeserializeObject(respString) as dynamic)?["data"];
                model.ClientName = data?["client_name"] != null ? data["client_name"] : "Unknown";
                model.ClientImageUrl = data?["img_url"];
                model.ClientEmailId = data?["email"];
            }
            else
                model.ClientName = "Unknown";

            respString = await InterServiceCommunication.GetAsync("http://52.168.38.130/api/patients/" + model.PatientId, token);

            if (respString != null)
            {
                var data = (JsonConvert.DeserializeObject(respString) as dynamic)?["data"];
                model.PatientName = data?["patient_name"] != null ? data["patient_name"] : "Unknown";
                model.PatientImageUrl = data?["img_url"];
            }
            else
                model.PatientName = "Unknown";

            respString = await InterServiceCommunication.GetAsync(ServiceAddress.ResourceSearchUrl + model.DoctorId, token);

            if (respString != null)
            {
                var data = (JsonConvert.DeserializeObject(respString) as dynamic)?["data"];
                model.DoctorName = data?["resource_name"] != null ? data["resource_name"] : "Unknown";
            }
            else
                model.DoctorName = "Unknown";
        }

        public IEnumerable<AppointmentModel> SearchModelByResourceIdAndDate(AppointmentModel item, DateTime date)
        {
            //List<AppointmentModel> itemList = new List<AppointmentModel>();

            var stopwatch = new Stopwatch();
            stopwatch.Start();

            var itemList = this.FilterSearchByResourceIdAndDate(item, date).ToList();

            stopwatch.Stop();
            this.logHandler.LogMetric(this.GetType().ToString() + " SearchModelByResourceIdAndDate", stopwatch.ElapsedMilliseconds);

            return itemList;
        }

        private async Task NotifyResource(int resourceId, object payload, string token, string operation)
        {
            try
            {
                var resourceResult = await InterServiceCommunication.GetAsync(ServiceAddress.ResourceSearchUrl + resourceId.ToString(), token);

                if (resourceResult != null)
                {
                    var data = (JsonConvert.DeserializeObject(resourceResult) as dynamic)?["data"];
                    if (data?["email"] != null)
                    {
                        string email = data["email"];
                        SendNotification(new List<string>() { email }, payload, token, operation);
                    }
                }

            }
            catch (Exception e)
            {
                Console.Error.WriteLine("Exception " + e.Message);
            }
        }

        private Task SendNotification(List<string> UsersName, object payload, string token, string eventCode)
        {
            try
            {
                return Task.Run(() =>
                {
                    var json = JsonConvert.SerializeObject(payload);
                    string url = string.Empty;

                    List<Dictionary<string, string>> dicPayload = JsonConvert.DeserializeObject<List<Dictionary<string, string>>>(json);

                    Notification notification = new Notification(UsersName, eventCode, dicPayload);

                    // No need to wait for notification to complete
                    InterServiceCommunication.PostAsync(ServiceAddress.NotificationsUrl, token, notification);

                    return;
                });
            }
            catch (Exception e)
            {
                Console.Error.WriteLine("Exception " + e.Message);
            }

            return null;
        }

        private async Task SendEmailNotification(string fromEmail, int toId, string subject, string body, string token, string ccEmailAddress = "")
        {
            try
            {
                var clientResult = await InterServiceCommunication.GetAsync(ServiceAddress.ClientSearchUrl + toId.ToString(), token);

                if (clientResult != null)
                {
                    var data = (JsonConvert.DeserializeObject(clientResult) as dynamic)?["data"];
                    if (data?["email"] != null)
                    {
                        string email = data["email"];
                        SendMailRequest notification = new SendMailRequest("", ccEmailAddress, email, subject, body, "", "");
                        // No need to wait for notification to complete
                        InterServiceCommunication.PostAsync(ServiceAddress.EmailNotificationUrl, token, notification);
                    }
                }
            }
            catch (Exception e)
            {
                Console.Error.WriteLine("Exception " + e.Message);
            }
        }
    }
}
