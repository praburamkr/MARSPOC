using Common.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Resource.Models
{
    [Table("Availability")]
    public class AvailabilityModel : IModel
    {
        [Key]
        public int Id { get; set; }

        [Column(TypeName = "Date")]
        public DateTime Date { get; set; }

        public TimeSpan StartTime { get; set; }

        public TimeSpan EndTime { get; set; }

        public int ResourceId { get; set; }

        public void Copy(IModel item)
        {
            if (item is AvailabilityModel)
            {
                AvailabilityModel data = item as AvailabilityModel;
                this.Id = data.Id;
                this.Date = data.Date;
                this.StartTime = data.StartTime;
                this.EndTime = data.EndTime;
                this.ResourceId = data.ResourceId;
            }
        }
    }
}
