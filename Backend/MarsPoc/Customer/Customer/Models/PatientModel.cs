using Common.Interfaces;
using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Customer.Models
{
    [Table("Patients")]
    public class PatientModel : IModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        [Column("patient_id")]
        [JsonProperty("patient_id")]
        public int Id { get; set; }

        [Column("client_id")]
        [JsonProperty("client_id")]
        public int ClientId { get; set; }

        [Column("patient_name")]
        [JsonProperty("patient_name")]
        public string Name { get; set; }

       [Column("species_id")]
        [JsonProperty("species_id")]
        public int SpeciesId { get; set; }

        [Column("species_name")]
        [JsonProperty("species_name")]
        public string SpeciesName { get; set; }

        [Column("age")]
        [JsonProperty("age")]
        public double Age { get; set; }
        
        [Column("weight")]
        [JsonProperty("weight")]
        public double Weight { get; set; }

        [Column("color_pattern")]
        [JsonProperty("color_pattern")]
        public string ColorPattern { get; set; }

        [Column("sex")]
        [JsonProperty("sex")]
        public string Sex { get; set; }

        [Column("breed")]
        [JsonProperty("breed")]
        public string Breed { get; set; }

        [Column("birth_date")]
        [JsonProperty("birth_date")]
        public DateTime BirthDate { get; set; }

        [Column("weight_uom")]
        [JsonProperty("weight_uom")]
        public string WeightUOM { get; set; }

        [Column("pref_doctor")]
        [JsonProperty("pref_doctor")]
        public int PrefDoctor { get; set; }

        [Column("img_url")]
        [JsonProperty("img_url")]
        public string ImageUrl { get; set; }

        public void Copy(IModel item)
        {
            if (item is PatientModel)
            {
                PatientModel data = item as PatientModel;
                this.Id = data.Id;
                this.ClientId = data.ClientId;
                this.Name = data.Name;
                this.SpeciesId = data.SpeciesId;
                this.SpeciesName = data.SpeciesName;
                this.Age = data.Age;
                this.Weight = data.Weight;
                this.ColorPattern = data.ColorPattern;
                this.Sex = data.Sex;
                this.Breed = data.Breed;
                this.BirthDate = data.BirthDate;
                this.WeightUOM = data.WeightUOM;
                this.PrefDoctor = data.PrefDoctor;
                this.ImageUrl = data.ImageUrl;
            }
        }
    }
}
