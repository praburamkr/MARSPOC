using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Appointment.Models;
using Common.Models;
using Microsoft.AspNetCore.Mvc;
using Common.Communication;
using Microsoft.AspNetCore.Http;
using Common.Base;
using Common.Interfaces;
using Appointment.Repositories;
using Newtonsoft.Json;
using Microsoft.Extensions.Configuration;

namespace Appointment.Repositories 
{

    public class AvailabilitySearchRepository : IAvailabilitySearch
    {
        private readonly AppointmentRepository appointmentContext;
        private readonly AppointmentTypeRepository appointmentTypeContext;
        private readonly ILogHandler logHandler;
        private readonly IConfiguration configuration;

        public AvailabilitySearchRepository(
            AppointmentRepository context,
            AppointmentTypeRepository appointmentTypeContext,
            ILogHandler logHandler, 
            IConfiguration configuration)
        {
            this.appointmentContext = context;
            this.appointmentTypeContext = appointmentTypeContext;
            this.configuration = configuration;
        }

        public async Task<MarsResponse> SearchAvailabilityAsync(AvailabilitySerachModel availabilitySearch, String token)
        {

            string errMsg = string.Empty;
            //Validate input
            errMsg = await ValidateAvailabilitySearchCriteriaAsync(availabilitySearch.PatientId, availabilitySearch.AppointmentDate);
            if (!string.IsNullOrEmpty(errMsg))
            {
                var errResp = MarsResponse.GetResponse(string.Empty, HttpStatusCode.BadRequest);
                    errResp.Error = new MarsException(errMsg);
                return errResp;
            }

            //Fetch prefered doctor for given patient id from marsstorage.[dbo].[Patients] tablle.
            string patientsUrl = ServiceAddress.PatientsBaseUrl + availabilitySearch.PatientId;
            var patient = await InterServiceCommunication.GetAsync(patientsUrl, token);
            int prefDoctorID = 0;

            if (patient != null)
            {
                dynamic patientData = JsonConvert.DeserializeObject(patient);
                var data = patientData["data"];

                if (data["pref_doctor"] != null)
                    prefDoctorID = data["pref_doctor"];
            }

            //Fetch doctor availability from marspoc.dbo.[Availability] 
            string resourcesUrl = ServiceAddress.AvailabilityBaseUrl;   //"http://localhost:5000/api/resource/availability/";   //
            var availability = string.Empty;
            if (prefDoctorID > 0)
            {
                var body = new AvailabilityRequest
                {
                    Id = 0,
                    Date = availabilitySearch.AppointmentDate.Date,
                    StartTime = new TimeSpan(),
                    EndTime = new TimeSpan(),
                    ResourceId = prefDoctorID
                };

                availability = await InterServiceCommunication.PostAsync(resourcesUrl, token, body);
            }
            dynamic availabilityData = JsonConvert.DeserializeObject(availability);
            dynamic avData = null;
            if (!string.IsNullOrEmpty(availability))
                avData = availabilityData["data"];

            if (prefDoctorID == 0 || avData.First == null)
            {
                availability = await InterServiceCommunication.GetAsync(resourcesUrl, token);
                availabilityData = JsonConvert.DeserializeObject(availability);
                if (!string.IsNullOrEmpty(availability))
                    avData = availabilityData["data"];
            }

            DateTime startTime;// = new DateTime();
            DateTime endTime;
            String doctorName = string.Empty;
            int docId = 0;
            //DateTime bookingTime;
            TimeSpan hospitalStartTime;
            TimeSpan hospitalEndTime;
            TimeSpan.TryParse(configuration.GetSection("Hospital").GetSection("StartTime").Value, out hospitalStartTime);
            TimeSpan.TryParse(configuration.GetValue<string>("Hospital:EndTime"), out hospitalEndTime);
            List<AvailabilityModel> timeslots = new List<AvailabilityModel>();

            if (!string.IsNullOrEmpty(availability) && avData.First != null)
            {
                //Fetch default time for given appointment type from master data(mars_appointments.[dbo].[appointment_types])
                int defaultDuration = availabilitySearch.SubTypeId > 0? GetDefaultDurationOfApptType(availabilitySearch.SubTypeId) : 30;                

                if (defaultDuration > 0)
                {
                    //dynamic availabilityData = JsonConvert.DeserializeObject(availability);
                    //dynamic data = availabilityData["data"];
                    //List<AvailabilityModel> availableResources = JsonConvert.DeserializeObject<List<AvailabilityModel>>(JsonConvert.SerializeObject(data));
                    
                    foreach (dynamic d in avData)
                    {
                        if (timeslots.Count > 0) break;

                        //startTime = ((DateTime)d["date"]).Add(((TimeSpan)d["startTime"] > hospitalStartTime && (TimeSpan)d["startTime"] < hospitalEndTime) ? (TimeSpan)d["startTime"] : hospitalStartTime);
                        //endTime = ((DateTime)d["date"]).Add( (TimeSpan)d["endTime"] );
                        startTime = ((DateTime)d["date"]).Add((TimeSpan)d["startTime"]);
                        endTime = ((DateTime)d["date"]).Add((TimeSpan)d["endTime"]);
                        docId = d["resourceId"];
                        doctorName = d["name"];

                        //bookingTime = new DateTime(1970, 1, 1).AddMilliseconds(availabilitySearch.ClientReqTime).ToLocalTime();// .AddHours(-7);   //setting to PST time   // .ToLocalTime();
                        DateTime bookingTime = RoundUp(DateTime.Parse(DateTime.Now.ToString()), TimeSpan.FromMinutes(30));
                        if (bookingTime >= endTime)
                            continue;
                        else if (bookingTime > startTime && bookingTime < endTime)
                        {
                            //string s = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss");
                            //string st = String.Format("{0:s}", DateTime.Now);
                            //string st = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:00");                            
                            startTime = Convert.ToDateTime(bookingTime.ToString("yyyy-MM-ddTHH:mm:00"));   //startTime = startTime.Add(DateTime.Now.AddSeconds(-DateTime.Now.Second).TimeOfDay);                            
                            if (endTime.Subtract(startTime).TotalMinutes < defaultDuration)
                                continue; 
                        }                            

                        //Fetch Appointments for given resource from mars_appointment.[dbo].[appointments] table
                        AppointmentModel appModel = new AppointmentModel
                        {
                            DoctorId = docId    //prefDoctorID > 0 ? prefDoctorID : docId
                        };
                        var appointments = this.appointmentContext.SearchModelByResourceIdAndDate(appModel, startTime.Date);
                        if (appointments.ToList().Count > 1)
                            appointments = appointments.Where(a => a.EndTime >= startTime && a.StartTime <= endTime).OrderBy(a => a.StartTime);
                        

                        //Find availability based on default duration of appointment type and already booked appointments if any. 
                        if (availabilitySearch.DefaultSlot)
                            timeslots = FindDefaultTimeslot(appointments.ToList(), startTime, endTime, defaultDuration);
                        else
                            timeslots = FindAllAvailableTimeslots(appointments.ToList(), startTime, endTime, defaultDuration);


                        if (timeslots != null && timeslots.Count > 0)
                        {
                            foreach (var t in timeslots)
                            {
                                t.DoctorId = docId;
                                t.DoctorName = doctorName;
                                t.AppointmentDate = startTime.Date;
                            }
                            //if (availabilitySearch.DefaultSlot || timeslots.Count == 1)
                            //{
                            //    timeslots.FirstOrDefault().DoctorId = prefDoctorID;
                            //    timeslots.FirstOrDefault().AppointmentDate = availabilitySearch.AppointmentDate.Date;
                            //    timeslots.FirstOrDefault().DoctorName = doctorName;
                            //}
                        }
                    }
                }                
            }            

            MarsResponse resp = MarsResponse.GetResponse(timeslots);
            if (timeslots == null)
                resp.Code = HttpStatusCode.NotAcceptable;
            else if (timeslots.Count() == 0)
                //resp.Code = HttpStatusCode.Accepted;
                resp.Error = new MarsException("Time slots not found.");

            return resp;
        }

        DateTime RoundUp(DateTime dt, TimeSpan d)
        {
            return new DateTime((dt.Ticks + d.Ticks - 1) / d.Ticks * d.Ticks, dt.Kind);
        }

        public static DateTime ConvertFromUnixTimestamp(double timestamp)
        {
            DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Local);
            return origin.AddSeconds(timestamp/1000);
        }

        private int GetDefaultDurationOfApptType (int type)
        {
            var duration = (from aptType in appointmentTypeContext.ModelSet
                            where aptType.Id == type
                            select new{
                                aptType.DefaultDuration
                            });
            return (duration != null && duration.ToList().Count > 0) ? duration.FirstOrDefault().DefaultDuration : 0;
        }

        private List<AvailabilityModel> FindDefaultTimeslot(List<AppointmentModel> appointments, DateTime startTime , DateTime endTime, int defaultDuration)
        {
            List<AvailabilityModel> timeslots = new List<AvailabilityModel>();

            int startTimeInMin = (startTime.Hour * 60) + startTime.Minute;
            int endTimeInMin = (endTime.Hour * 60) + endTime.Minute;
            int firstAptStartTimeInMin = 0;
            int lastAptEndTimeInMin = 0;
            if (appointments.Count > 0)
            {
                firstAptStartTimeInMin = (appointments.FirstOrDefault().StartTime.Hour * 60) + appointments.FirstOrDefault().StartTime.Minute;
                lastAptEndTimeInMin = (appointments.LastOrDefault().EndTime.Hour * 60) + appointments.LastOrDefault().EndTime.Minute;
            }
            
            if ((int)(endTime.Subtract(startTime).TotalMinutes) > defaultDuration)
            {                
                if (appointments.Count == 0 || (startTimeInMin < firstAptStartTimeInMin) && ((firstAptStartTimeInMin - startTimeInMin) > defaultDuration))
                {
                    timeslots.Add(new AvailabilityModel
                    {
                        StartTime = startTime,
                        EndTime = startTime.AddMinutes(defaultDuration),
                        Duration = defaultDuration
                        
                    });
                }                
                else if (appointments.Count > 0)     //lastAptEndTimeInMin + defaultDuration > endTimeInMin)
                {
                    DateTime timeslotStart = DateTime.MinValue;
                    DateTime timeslotEnd = DateTime.MinValue;
                    for (int i = 0; i < appointments.Count - 1; i++)
                    {
                        if(appointments.ElementAt(i+1).StartTime.Subtract(appointments.ElementAt(i).EndTime).TotalMinutes > defaultDuration)
                        {
                            //FixAppointment(appointments.ElementAt(i).EndTime, appointments.ElementAt(i).EndTime + defaultDuration);
                            timeslots.Add(new AvailabilityModel
                            {
                                StartTime = appointments.ElementAt(i).EndTime,
                                EndTime = appointments.ElementAt(i).EndTime.AddMinutes(defaultDuration),
                                Duration = defaultDuration

                            });
                            break;
                        }
                    }
                }
                if (timeslots.Count == 0 && ((lastAptEndTimeInMin < endTimeInMin) && ((endTimeInMin - lastAptEndTimeInMin) > defaultDuration)))
                {
                    timeslots.Add(new AvailabilityModel
                    {
                        StartTime = appointments.LastOrDefault().EndTime,
                        EndTime = appointments.LastOrDefault().EndTime.AddMinutes(defaultDuration),
                        Duration = defaultDuration
                    });
                }
            }

            return timeslots;          
        }

        private List<AvailabilityModel> FindAllAvailableTimeslots(List<AppointmentModel> appointments, DateTime startTime, DateTime endTime, int defaultDuration)
        {
            List<AvailabilityModel> timeslots = new List<AvailabilityModel>();

            int startTimeInMin = (startTime.Hour * 60) + startTime.Minute;
            int endTimeInMin = (endTime.Hour * 60) + endTime.Minute;
            int firstAptStartTimeInMin = 0;
            int lastAptEndTimeInMin = 0;
            if (appointments.Count > 0)
            {
                firstAptStartTimeInMin = (appointments.FirstOrDefault().StartTime.Hour * 60) + appointments.FirstOrDefault().StartTime.Minute;
                lastAptEndTimeInMin = (appointments.LastOrDefault().EndTime.Hour * 60) + appointments.LastOrDefault().EndTime.Minute;
            }

            if ((int)(endTime.Subtract(startTime).TotalMinutes) > defaultDuration)
            {
                if (appointments.Count == 0)
                {
                    timeslots.Add(new AvailabilityModel
                    {
                        StartTime = startTime,
                        EndTime = endTime  //startTime.AddMinutes(defaultDuration)
                    });
                }
                else if (appointments.Count > 0)
                {
                    if ((startTimeInMin < firstAptStartTimeInMin) && ((firstAptStartTimeInMin - startTimeInMin) > defaultDuration))
                    {
                        timeslots.Add(new AvailabilityModel
                        {
                            StartTime = startTime,
                            EndTime = startTime.AddMinutes(firstAptStartTimeInMin - startTimeInMin)    //startTime.AddMinutes(defaultDuration)
                        });
                    }
                    if (appointments.Count > 1)
                    {
                        for (int i = 0; i < appointments.Count - 1; i++)
                        {
                            if (appointments.ElementAt(i + 1).StartTime.Subtract(appointments.ElementAt(i).EndTime).TotalMinutes > defaultDuration)
                            {
                                //FixAppointment(appointments.ElementAt(i).EndTime, appointments.ElementAt(i).EndTime + defaultDuration);
                                timeslots.Add(new AvailabilityModel
                                {
                                    StartTime = appointments.ElementAt(i).EndTime,
                                    EndTime = appointments.ElementAt(i + 1).StartTime  // appointments.ElementAt(i).EndTime.AddMinutes(defaultDuration)
                                });
                            }
                        }
                    }                    

                    if ((lastAptEndTimeInMin < endTimeInMin) && ((endTimeInMin - lastAptEndTimeInMin) > defaultDuration))
                    {
                        timeslots.Add(new AvailabilityModel
                        {
                            StartTime = appointments.LastOrDefault().EndTime,
                            EndTime = endTime   // appointments.LastOrDefault().EndTime.AddMinutes(defaultDuration)
                        });
                    }
                }                
            }
            return timeslots;
        }

        private async Task<string> ValidateAvailabilitySearchCriteriaAsync(int patientId, DateTime appointmentDate)    //, int apmtType, DateTime date)
        {
            string invalidReqMsg = string.Empty;

            await (Task.Run(() =>
            {
                if (patientId <= 0)
                    invalidReqMsg = "Invalid data: patient_id is mandotory!";
                else if (DateTime.Compare(appointmentDate, DateTime.Now.Date) < 0)
                    invalidReqMsg = "Invalid data: Appointment date should be current date or later but not previous date.!";
            }));            

            return invalidReqMsg;
        }

        //private async Task<int> mymethodAsync()

        private string ValidateInputAsync(int patientId, DateTime appointmentDate)
        {
            string invalidReqMsg = string.Empty;
            if (patientId <= 0)
                invalidReqMsg = "Invalid data: patient_id is mandotory!";
            else if (DateTime.Compare(appointmentDate, DateTime.Now.Date) < 0)
                invalidReqMsg = "Invalid data: Appointment date should be current date or later but not previous date.!";

            return invalidReqMsg;
        }

        private AvailabilityModel FixAppointment(DateTime startTime, DateTime endTime)
        {
            return new AvailabilityModel
            {
                StartTime = startTime,
                EndTime = endTime
            };
        }


        //to be deleted
        public async Task<IActionResult> SendResponse(ControllerBase controller, Task<MarsResponse> response)
        {
            return null;
        }

    }
}
