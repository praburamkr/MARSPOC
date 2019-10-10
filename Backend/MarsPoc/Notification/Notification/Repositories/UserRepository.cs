using Common.Base;
using Common.Interfaces;
using Common.Models;
using Microsoft.EntityFrameworkCore;
using Notification.Models;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Notification.Repositories
{
    public class UserRepository : RepositoryBase<UserModel, UserRepository>
    {
        private readonly ILogHandler logHandler;

        public UserRepository(DbContextOptions<UserRepository> options, ILogHandler logHandler) : base(options, logHandler)
        {
            this.logHandler = logHandler;
        }

        protected override IEnumerable<UserModel> FilterSearch(UserModel item)
        {
            IEnumerable<UserModel> userList = null;
            userList = this.ModelSet.Where(u => u.UserId == item.UserId).ToArray();
            return userList;
        }

        public IDictionary<string, string> FilterSearchByUserList(List<string> users)
        {
            Dictionary<string, string> userLists = new Dictionary<string, string>();
            foreach (var user in users)
            {
                var item = this.ModelSet.Where(u => u.UserId == user).ToArray();
                if (item != null)
                {
                    userLists.Add(item.ToList()[0].UserId, item.ToList()[0].ClientType);
                }
            }
            return userLists;
        }

        public async Task<MarsResponse> CreateOrUpdateAsync(UserModel model)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            var item = await this.ModelSet.SingleOrDefaultAsync(c => c.UserId == model.UserId);

            if (item == null)
            {
                await this.ModelSet.AddAsync(model);
                item = model;
            }
            else
            {
                var id = item.Id;
                item.Copy(model);
                item.Id = id;
            }

            await this.SaveChangesAsync();

            stopwatch.Stop();
            this.logHandler.LogMetric(this.GetType().ToString() + " CreateOrUpdateAsync", stopwatch.ElapsedMilliseconds);

            return MarsResponse.GetResponse(model, HttpStatusCode.Created);
        }

    }
}
