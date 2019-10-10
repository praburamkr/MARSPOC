using Common.Interfaces;
using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Appointment.Models
{
    [Table("Appointments")]
    public class AppointmentModel : IModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("appointment_id")]
        [JsonProperty("appointment_id")]
        public int Id { get; set; }

        [Column("appt_scan_id")]
        [JsonProperty("appt_scan_id")]
        public string ScanId { get; set; }

        [Column("appt_link_id")]
        [JsonProperty("appt_link_id")]
        public int LinkId { get; set; }

        [Column("appt_type_id")]
        [JsonProperty("appt_type_id")]
        public int TypeId { get; set; }

        [Column("appt_sub_type_id")]
        [JsonProperty("appt_sub_type_id")]
        public int SubTypeId { get; set; }

        [NotMapped]
        [JsonProperty("appt_type")]
        public AppointmentTypeModel AppointmentType { get; set; }

        [Column("client_id")]
        [JsonProperty("client_id")]
        public int ClientId { get; set; }

        [NotMapped]
        [JsonProperty("client_name")]
        public string ClientName { get; set; }

        [NotMapped]
        [JsonProperty("client_email")]
        public string ClientEmailId { get; set; }

        [NotMapped]
        [JsonProperty("client_img_url")]
        public string ClientImageUrl { get; set; }

        [Column("patient_id")]
        [JsonProperty("patient_id")]
        public int PatientId { get; set; }

        [NotMapped]
        [JsonProperty("patient_name")]
        public string PatientName { get; set; }

        [NotMapped]
        [JsonProperty("patient_img_url")]
        public string PatientImageUrl { get; set; }

        [Column("appt_start_time")]
        [JsonProperty("appt_start_time")]
        public DateTime StartTime { get; set; }

        [Column("appt_end_time")]
        [JsonProperty("appt_end_time")]
        public DateTime EndTime { get; set; }

        [NotMapped]
        [JsonProperty("appt_response")]
        public string Response { get; set; }

        [Column("doctor_id")]
        [JsonProperty("doctor_id")]
        public int DoctorId { get; set; }

        [NotMapped]
        [JsonProperty("doctor_name")]
        public string DoctorName { get; set; }

        [Column("duration")]
        [JsonProperty("duration")]
        public int Duration { get; set; }

        [Column("reason_code_id")]
        [JsonProperty("reason_code_id")]
        public int ReasonId { get; set; }

        [NotMapped]
        [JsonProperty("reason")]
        public ReasonCodeModel Reason { get; set; }

        [Column("note_for_doctor")]
        [JsonProperty("note_for_doctor")]
        public string DoctorNote { get; set; }

        [Column("appt_status")]
        [JsonProperty("appt_status")]
        public int Status { get; set; }

        [Column("appt_workflow_state")]
        [JsonProperty("appt_workflow_state")]
        public int WorkflowState { get; set; }

        [Column("is_email")]
        [JsonProperty("is_email")]
        public bool IsEmail { get; set; }

        [Column("email")]
        [JsonProperty("email")]
        public string Email { get; set; }

        public void Copy(IModel item)
        {
            if (item is AppointmentModel)
            {
                AppointmentModel data = item as AppointmentModel;
                this.Id = data.Id;
                this.StartTime = data.StartTime;
                this.ScanId = data.ScanId;
                this.LinkId = data.LinkId;
                this.TypeId = data.TypeId;
                this.SubTypeId = data.SubTypeId;
                this.EndTime = data.EndTime;
                this.DoctorId = data.DoctorId;
                this.ClientName = data.ClientName;
                //this.Type = data.Type;
                //this.ClientId = data.ClientId;
                this.PatientId = data.PatientId;
                this.PatientName = data.PatientName;
                this.DoctorName = data.DoctorName;
                this.Duration = data.Duration;
                this.ReasonId = data.ReasonId;
                this.Reason = data.Reason;
                this.DoctorNote = data.DoctorNote;
                this.WorkflowState = data.WorkflowState;
                this.Status = data.Status;
            }
        }
    }
}
