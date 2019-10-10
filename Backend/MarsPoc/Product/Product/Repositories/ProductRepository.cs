using Common.Base;
using Common.Interfaces;
using Common.Models;
using Microsoft.EntityFrameworkCore;
using Product.Models;
using System.Collections.Generic;
using System.Linq;

namespace Product.Repositories
{
    public class ProductRepository : RepositoryBase<ProductModel, ProductRepository>
    {
        private readonly ModelEqualityComparer<ProductModel> modelEqualityComparer;

        public ProductRepository(DbContextOptions<ProductRepository> options, ILogHandler logHandler) : base(options, logHandler)
        {
            modelEqualityComparer = new ModelEqualityComparer<ProductModel>();
        }

        protected override IEnumerable<ProductModel> FilterSearch(ProductModel item)
        {
            IEnumerable<ProductModel> productList = null;

            if (!string.IsNullOrWhiteSpace(item.Name))
                productList = this.ModelSet.Where(c => c.Name.Contains(item.Name)).ToArray();

            if (!string.IsNullOrWhiteSpace(item.ProductType))
            {
                if (productList == null)
                    productList = this.ModelSet.Where(c => c.ProductType.Contains(item.ProductType)).ToArray();
                else
                    productList = productList.Union(this.ModelSet.Where(c => c.ProductType.Contains(item.ProductType)).ToArray(), modelEqualityComparer);
            }

            return productList;
        }
    }
}
