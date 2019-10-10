using Common.Base;
using Common.Interfaces;
using Common.Models;
using Customer.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Customer.Repositories
{
    public class PatientRepository : RepositoryBase<PatientModel, PatientRepository>
    {
        private readonly ILogHandler logHandler;

        public PatientRepository(DbContextOptions<PatientRepository> options, ILogHandler logHandler) : base(options, logHandler)
        {
            this.logHandler = logHandler;
        }

        protected override IEnumerable<PatientModel> FilterSearch(PatientModel item)
        {
            IEnumerable<PatientModel> patientList = null;

            patientList = this.ModelSet.Where(c => c.ClientId == item.ClientId).ToArray();

            return patientList;
        }

        public IEnumerable<PatientModel> SearchPatients(int clientId)
        {
            IEnumerable<PatientModel> patientList = null;

            patientList = this.ModelSet.Where(c => c.ClientId == clientId).ToArray();

            return patientList;
        }

        public override async Task<IEnumerable<object>> SearchAllModelAsync(IEnumerable<int> idList)
        {
            if (idList == null || !idList.Any())
                return null;

            return await Task.Run(() =>
            {
                var itemList = (from item in this.ModelSet
                               where idList.Contains(item.Id)
                               select new { item.Id, item.Name, item.ImageUrl }).ToArray();
                return itemList;
            });
        }

        public async Task<MarsResponse> CreateAsync(PatientModel item)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            if (string.IsNullOrEmpty(item.ImageUrl))
            {
                if (!string.IsNullOrEmpty(item.SpeciesName))
                {
                    if (item.SpeciesName.ToUpper().CompareTo("CAT") == 0)
                    {
                        item.ImageUrl = DefaultPatientsUrl.DEFAULT_CAT;
                    }
                    else if (item.SpeciesName.ToUpper().CompareTo("DOG") == 0)
                    {
                        item.ImageUrl = DefaultPatientsUrl.DEFAULT_DOG;
                    }
                    else
                    {
                        item.ImageUrl = DefaultPatientsUrl.DEFAULT_IMG;
                    }
                }
                else
                {
                    item.ImageUrl = DefaultPatientsUrl.DEFAULT_IMG;
                }
            }

            await this.CreateModelAsync(item);

            stopwatch.Stop();
            this.logHandler.LogMetric(this.GetType().ToString() + " CreateAsync", stopwatch.ElapsedMilliseconds);

            return MarsResponse.GetResponse(item, HttpStatusCode.Created);
        }
    }
}
