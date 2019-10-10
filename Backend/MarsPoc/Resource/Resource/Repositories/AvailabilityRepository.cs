using Common.Base;
using Common.Interfaces;
using Common.Models;
using Microsoft.EntityFrameworkCore;
using Resource.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Resource.Repositories
{
    public class AvailabilityRepository : RepositoryBase<AvailabilityModel, AvailabilityRepository>
    {

        private readonly ModelEqualityComparer<AvailabilityModel> modelEqualityComparer;

        public AvailabilityRepository(DbContextOptions<AvailabilityRepository> options, ILogHandler logHandler) : base(options, logHandler) 
        {
            modelEqualityComparer = new ModelEqualityComparer<AvailabilityModel>();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public async Task<MarsResponse> GetAllAsyncAsPerDateNResourceId(AvailabilityModel  item)
        {
            var resourceList = await Task.Run(() => this.ModelSet.Where(m=>m.ResourceId == item.ResourceId && DateTime.Compare(m.Date, item.Date) == 0).ToArray());
            HttpStatusCode code = HttpStatusCode.OK;
            if (resourceList == null)
                code = HttpStatusCode.NotFound;
            return MarsResponse.GetResponse(resourceList, code);
        }



        public async Task<IEnumerable<AvailabilityModel>> GetAsync(AvailabilityModel objAvailabilityModel)
        {
            return await Task.Run(() => FilterSearch(objAvailabilityModel));
        }

        public async Task<IEnumerable<AvailabilityModel>> GetAllAsync(DateTime date)
        {
            return await Task.Run(() => FilterSearchFromDate(date));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        protected override IEnumerable<AvailabilityModel> FilterSearch(AvailabilityModel item)
        {
            IEnumerable<AvailabilityModel> resourceList = null;            

            if (item.Date != null)
                resourceList =  this.ModelSet.Where(c => DateTime.Compare(c.Date, item.Date) == 0).ToArray();

            if (item.ResourceId != 0)
            {
                if (resourceList == null)
                    resourceList = this.ModelSet.Where(c => c.ResourceId == item.ResourceId).ToArray();
                else
                    resourceList = resourceList.Union(this.ModelSet.Where(c => c.ResourceId == item.ResourceId).ToArray(), modelEqualityComparer);
            }

            if (item.ResourceId != 0 && item.Date != null)
            {
                resourceList = this.ModelSet.Where(m => m.ResourceId == item.ResourceId && DateTime.Compare(m.Date, item.Date) >= 0).OrderBy(r => r.Date).ToArray();
                //if (!resourceList.Any()) resourceList = this.ModelSet.Where(c => c.Date > DateTime.Now.Date);
            }

            if (item.StartTime != TimeSpan.Zero && item.EndTime == TimeSpan.Zero)
            {
                resourceList = this.ModelSet.Where(c => c.StartTime >= item.StartTime && c.EndTime <= item.EndTime).ToArray();
            }

            return resourceList;
        }

        protected IEnumerable<AvailabilityModel> FilterSearchFromDate(DateTime date)
        {
            IEnumerable<AvailabilityModel> resourceList = null;
            resourceList = this.ModelSet.Where(c => DateTime.Compare(c.Date, date) >= 0).OrderBy(r => r.Date).ToArray();
            return resourceList;
        }

    }
}
