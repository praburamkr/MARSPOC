using Common.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Resource.Models
{
    [Table("resources")]
    public class ResourceModel : IModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("resource_id")]
        [JsonProperty("resource_id")]
        public int Id { get; set; }

        [Column("resource_type")]
        [JsonProperty("resource_type")]
        public int ResourceType { get; set; }

        [Column("resource_name")]
        [JsonProperty("resource_name")]
        public string ResourceName { get; set; }

        [Column("email")]
        [JsonProperty("email")]
        public string ResourceEmail { get; set; }

        [Column("img_url")]
        [JsonProperty("img_url")]
        public string ImageUrl { get; set; }

        public void Copy(IModel item)
        {
            if (item is ResourceModel)
            {
                ResourceModel data = item as ResourceModel;
                this.Id = data.Id;
                this.ResourceType = data.ResourceType;
                this.ResourceName = data.ResourceName;
                this.ImageUrl = data.ImageUrl;
            }
        }
    }
}
