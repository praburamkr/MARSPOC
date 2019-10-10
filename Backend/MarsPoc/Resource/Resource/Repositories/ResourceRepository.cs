using Common.Base;
using Common.Interfaces;
using Common.Models;
using Microsoft.EntityFrameworkCore;
using Resource.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Resource.Repositories
{
    public class ResourceRepository : RepositoryBase<ResourceModel, ResourceRepository>
    {
        private readonly ModelEqualityComparer<ResourceModel> modelEqualityComparer;

        public ResourceRepository(DbContextOptions<ResourceRepository> options, ILogHandler logHandler) : base(options, logHandler)
        {
            modelEqualityComparer = new ModelEqualityComparer<ResourceModel>();
        }

        public override async Task<IEnumerable<object>> SearchAllModelAsync(IEnumerable<int> idList)
        {
            if (idList == null || !idList.Any())
                return null;

            return await Task.Run(() =>
            {
                var itemList = (from item in this.ModelSet
                               where idList.Contains(item.Id)
                               select new { item.Id, item.ResourceName, item.ImageUrl }).ToArray();
                return itemList;
            });
        }

        protected override IEnumerable<ResourceModel> FilterSearch(ResourceModel item)
        {
            IEnumerable<ResourceModel> resourceList = null;

            if (!string.IsNullOrWhiteSpace(item.ResourceName))
                resourceList = this.ModelSet.Where(c => c.ResourceName.Contains(item.ResourceName)).ToArray();

            if (item.ResourceType != 0)
            {
                if (resourceList == null)
                    resourceList = this.ModelSet.Where(c => c.ResourceType == item.ResourceType).ToArray();
                else
                    resourceList = resourceList.Union(this.ModelSet.Where(c => c.ResourceType == item.ResourceType).ToArray(), modelEqualityComparer);
            }

            return resourceList;
        }
    }
}
