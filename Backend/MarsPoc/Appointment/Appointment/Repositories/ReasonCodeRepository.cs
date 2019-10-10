using Appointment.Models;
using Common.Base;
using Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Appointment.Repositories
{
    public class ReasonCodeRepository : RepositoryBase<ReasonCodeModel, ReasonCodeRepository>
    {
        private readonly ILogHandler logHandler;

        public ReasonCodeRepository(DbContextOptions<ReasonCodeRepository> options, ILogHandler logHandler) : base(options, logHandler)
        {
        }

        protected override IEnumerable<ReasonCodeModel> FilterSearch(ReasonCodeModel item)
        {
            IEnumerable<ReasonCodeModel> reasonList = this.ModelSet.ToArray();

            return reasonList;
        }
    }
}
