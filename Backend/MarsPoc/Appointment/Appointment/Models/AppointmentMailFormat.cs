using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Appointment.Models
{
    public class AppointmentMailFormat
    {
        public static string GetAppointmentMail(AppointmentModel appointment)
        {
            try
            {
                var lastFirstName = appointment.ClientName.Split(',');
                var file = File.ReadAllText(@"./Resource/MailHtml.txt");
                file = file.Replace("@ClientName", lastFirstName[0]);
                file = file.Replace("@PatientName", appointment.PatientName);
                file = file.Replace("@AppointmentDate", appointment.StartTime.ToString("ddd MM/dd/yyyy", CultureInfo.InvariantCulture));
                file = file.Replace("@StartTime", appointment.StartTime.ToString("hh:mm tt"));
                file = file.Replace("@EndTime", appointment.EndTime.ToString("hh:mm tt"));
                file = file.Replace("@DoctorName", appointment.DoctorName);


                if(!string.IsNullOrEmpty(appointment.DoctorNote))
                {
                    file = file.Replace("@Notes", appointment.DoctorNote);
                }
                else
                {
                    file = file.Replace("@Notes", string.Empty);
                }


                return file;
            }
            catch(Exception e)
            {
                return "";
            }
        }
    }
}