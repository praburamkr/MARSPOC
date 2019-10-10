using Common.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Authentication.Models
{
    [Table("ImageFileStore")]
    public class ImageFileStoreModel : IModel
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public Guid GuId { get; set; }

        public string Url { get; set; }

        public void Copy(IModel item)
        {
            if (item is ImageFileStoreModel)
            {
                ImageFileStoreModel model = new ImageFileStoreModel();
                this.Id = model.Id;
                this.Url = model.Url;
            }

        }
    }
}
