using Common.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Product.Models
{
    [Table("Products")]
    public class ProductModel : IModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ProductId")]
        public int Id { get; set; }

        public string Name { get; set; }

        public string ProductType { get; set; }

        public double Price { get; set; }

        public void Copy(IModel item)
        {
            if (item is ProductModel)
            {
                ProductModel data = item as ProductModel;
                this.Id = data.Id;
            }
        }
    }
}
