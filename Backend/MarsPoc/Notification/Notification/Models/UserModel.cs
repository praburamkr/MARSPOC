using Common.Interfaces;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Notification.Models
{
    [Table("Users")]
    public class UserModel : IModel
    {
       
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        [JsonProperty("id")]
        public int Id { get; set; }

        //[Required]
        [Key]
        [Column("user_id")]
        [JsonProperty("user_id")]
        public string UserId { get; set; }

        [Column("client_type")]
        [JsonProperty("client_type")]
        public string ClientType { get; set; }

        [Column("status")]
        [JsonProperty("status")]
        public string Status { get; set; }

        public void Copy(IModel item)
        {
            if (item is UserModel)
            {
                UserModel data = item as UserModel;
                this.Id = data.Id;
                this.UserId = data.UserId;
                this.ClientType = data.ClientType;
                this.Status = data.Status;
            }
        }
    }
}
