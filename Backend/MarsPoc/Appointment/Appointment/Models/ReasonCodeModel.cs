using Common.Interfaces;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Appointment.Models
{
    [Table("reason_codes")]
    public class ReasonCodeModel : IModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("reason_code_id")]
        [JsonProperty("reason_code_id")]
        public int Id { get; set; }

        [Column("appt_type_id")]
        [JsonProperty("appt_type_id")]
        public int AppointmentTypeId { get; set; }

        [NotMapped]
        [JsonIgnore]
        public AppointmentTypeModel AppointmentType { get; set; }

        [Column("reason_text")]
        [JsonProperty("reason_text")]
        public string Text { get; set; }

        public void Copy(IModel item)
        {
            if (item is ReasonCodeModel)
            {
                ReasonCodeModel data = item as ReasonCodeModel;
                this.Id = data.Id;
                this.AppointmentTypeId = data.AppointmentTypeId;
                this.AppointmentType = data.AppointmentType;
                this.Text = data.Text;
            }
        }
    }
}
