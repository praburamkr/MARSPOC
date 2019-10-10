using Common.Interfaces;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Appointment.Models
{
    [Table("appointment_types")]
    public class AppointmentTypeModel : IModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("appt_type_id")]
        [JsonProperty("appt_type_id")]
        public int Id { get; set; }

        [Column("parent_type_id")]
        [JsonProperty("parent_type_id")]
        public int ParentId { get; set; }

        [Column("appt_type_text")]
        [JsonProperty("appt_type_text")]
        public string Text { get; set; }

        [Column("default_duration")]
        [JsonProperty("default_duration")]
        public int DefaultDuration { get; set; }

        [Column("img_url")]
        [JsonProperty("img_url")]
        public string ImageUrl { get; set; }

        [Column("color")]
        [JsonProperty("color")]
        public string Color { get; set; }

        public void Copy(IModel item)
        {
            if (item is AppointmentTypeModel)
            {
                AppointmentTypeModel data = item as AppointmentTypeModel;
                this.Id = data.Id;
                this.ParentId = data.ParentId;
                this.Text = data.Text;
                this.DefaultDuration = data.DefaultDuration;
                this.ImageUrl = data.ImageUrl;
                this.Color = data.Color;
            }
        }
    }
}
