using Appointment.Models;
using Common.Base;
using Common.Interfaces;
using Common.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Appointment.Repositories
{
    public class AppointmentTypeRepository : RepositoryBase<AppointmentTypeModel, AppointmentTypeRepository>
    {
        private readonly ILogHandler logHandler;

        public AppointmentTypeRepository(DbContextOptions<AppointmentTypeRepository> options, ILogHandler logHandler) : base(options, logHandler)
        {
            this.logHandler = logHandler;
        }

        protected override IEnumerable<AppointmentTypeModel> FilterSearch(AppointmentTypeModel item)
        {
            IEnumerable<AppointmentTypeModel> apptTypeList = this.ModelSet.ToArray();

            return apptTypeList;
        }

        public async Task<MarsResponse> GetParentModelsAsync(int id)
        {
            return await Task.Run(() =>
            {
                var stopwatch = new Stopwatch();
                stopwatch.Start();

                var itemList = this.ModelSet.Where(c => c.ParentId == id).ToArray();

                stopwatch.Stop();
                this.logHandler.LogMetric(this.GetType().ToString() + " GetParentModelsAsync", stopwatch.ElapsedMilliseconds);

                HttpStatusCode code = HttpStatusCode.OK;
                if (itemList == null)
                    code = HttpStatusCode.NotFound;
                return MarsResponse.GetResponse(itemList, code);
            });
        }
    }
}
