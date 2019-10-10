using Common.Interfaces;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Customer.Models
{
    [Table("Clients")]
    public class CustomerModel : IModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("client_id")]
        [JsonProperty("client_id")]
        public int Id { get; set; }

        //[Required]
        [Column("client_name")]
        [JsonProperty("client_name")]
        public string Name { get; set; }

        [Column("address")]
        [JsonProperty("address")]
        public string Address { get; set; }

        [Column("phone")]
        [JsonProperty("phone")]
        public string ContactNumber { get; set; }

        [Column("email")]
        [JsonProperty("email")]
        public string Email { get; set; }

        [Column("img_url")]
        [JsonProperty("img_url")]
        public string ImageUrl { get; set; }

        [NotMapped]
        public PatientModel[] Patients { get; set; }

        public void Copy(IModel item)
        {
            if (item is CustomerModel)
            {
                CustomerModel data = item as CustomerModel;
                this.Id = data.Id;
                this.Name = data.Name;
                this.Address = data.Address;
                this.ContactNumber = data.ContactNumber;
                this.Email = data.Email;
                this.ImageUrl = data.ImageUrl;
            }
        }
    }
}
