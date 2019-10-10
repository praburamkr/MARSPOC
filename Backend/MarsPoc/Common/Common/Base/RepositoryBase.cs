using Common.Interfaces;
using Common.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using System.Diagnostics;

namespace Common.Base
{
    public abstract class RepositoryBase<TModel, TContext> : DbContext, IRepository<TModel>
        where TModel : class, IModel, new()
        where TContext : DbContext, IRepository<TModel>
    {
        private readonly ILogHandler logHandler;

        public RepositoryBase(DbContextOptions<TContext> options, ILogHandler logHandler) : base(options)
        {
            this.logHandler = logHandler;
        }

        public DbSet<TModel> ModelSet { get; set; }

        public async Task<MarsResponse> CreateAsync(TModel item)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            await this.CreateModelAsync(item);

            stopwatch.Stop();
            this.logHandler.LogMetric(this.GetType().ToString() + " CreateAsync", stopwatch.ElapsedMilliseconds);

            return MarsResponse.GetResponse(item, HttpStatusCode.Created);
        }

        public virtual async Task<TModel> CreateModelAsync(TModel item)
        {
            await this.ModelSet.AddAsync(item);
            await this.SaveChangesAsync();
            return item;
        }

        public async Task<MarsResponse> GetAsync(int id)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            var item = await this.GetModelAsync(id);

            stopwatch.Stop();
            this.logHandler.LogMetric(this.GetType().ToString() + " GetAsync", stopwatch.ElapsedMilliseconds);

            HttpStatusCode code = HttpStatusCode.OK;
            if (item == null)
                code = HttpStatusCode.NotFound;
            return MarsResponse.GetResponse(item, code);
        }

        public virtual async Task<TModel> GetModelAsync(int id)
        {
            var item = await this.ModelSet.SingleOrDefaultAsync(c => c.Id == id);
            return item;
        }

        public virtual async Task<MarsResponse> SearchAsync(TModel item)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            IEnumerable<TModel> itemList = null;
            await Task.Run(() =>
            {
                itemList = FilterSearch(item);
                MarsResponse resp = MarsResponse.GetResponse(itemList);
                if (itemList == null)
                    resp.Code = HttpStatusCode.NotAcceptable;
                else if (itemList.Count() == 0)
                    resp.Code = HttpStatusCode.NotFound;
            });
            
            stopwatch.Stop();
            this.logHandler.LogMetric(this.GetType().ToString() + " SearchAsync", stopwatch.ElapsedMilliseconds);

            return MarsResponse.GetResponse(itemList);
        }

        public virtual async Task<MarsResponse> SearchAsync(TModel item, string token)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            IEnumerable<TModel> itemList = null;
            await Task.Run(() =>
            {
                itemList = FilterSearch(item);
                MarsResponse resp = MarsResponse.GetResponse(itemList);
                if (itemList == null)
                    resp.Code = HttpStatusCode.NotAcceptable;
                else if (itemList.Count() == 0)
                    resp.Code = HttpStatusCode.NotFound;
            });

            stopwatch.Stop();
            this.logHandler.LogMetric(this.GetType().ToString() + " SearchAsync", stopwatch.ElapsedMilliseconds);

            return MarsResponse.GetResponse(itemList);
        }

        public virtual async Task<IEnumerable<TModel>> SearchModelAsync(TModel item)
        {
            IEnumerable<TModel> itemList = null;
            await Task.Run(() =>
            {
                itemList = FilterSearch(item);
            });
            return itemList;
        }

        protected abstract IEnumerable<TModel> FilterSearch(TModel item);

        public async Task<MarsResponse> SearchAllAsync(IEnumerable<int> idList)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            var itemList = await SearchAllModelAsync(idList);
            MarsResponse resp = MarsResponse.GetResponse(itemList);
            if (itemList == null)
                resp.Code = HttpStatusCode.NotAcceptable;
            else if (itemList.Count() == 0)
                resp.Code = HttpStatusCode.NotFound;

            stopwatch.Stop();
            this.logHandler.LogMetric(this.GetType().ToString() + " SearchAllAsync", stopwatch.ElapsedMilliseconds);

            return MarsResponse.GetResponse(itemList);
        }

        public virtual async Task<IEnumerable<object>> SearchAllModelAsync(IEnumerable<int> idList)
        {
            if (idList == null || !idList.Any())
                return null;

            IEnumerable<TModel> itemList = null;

            await Task.Run(() =>
            {
                itemList = this.ModelSet.Where(item => idList != null ? idList.Contains(item.Id) : false);
            });
            return itemList;
        }

        public async Task<MarsResponse> DeleteAsync(int id)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            var bDelete = await DeleteModelAsync(id);

            stopwatch.Stop();
            this.logHandler.LogMetric(this.GetType().ToString() + " DeleteAsync", stopwatch.ElapsedMilliseconds);

            return MarsResponse.GetResponse(bDelete, bDelete ? HttpStatusCode.OK : HttpStatusCode.NotFound);
        }

        public virtual async Task<bool> DeleteModelAsync(int id)
        {
            TModel item = await this.ModelSet.SingleOrDefaultAsync(c => c.Id == id);
            if (item == null)
                return false;

            this.ModelSet.Attach(item);
            this.ModelSet.Remove(item);
            await this.SaveChangesAsync();
            return true;
        }

        public async Task<MarsResponse> UpdateAsync(int id, TModel item)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            var mod = await UpdateModelAsync(id, item);

            stopwatch.Stop();
            this.logHandler.LogMetric(this.GetType().ToString() + " UpdateAsync", stopwatch.ElapsedMilliseconds);

            return MarsResponse.GetResponse(mod, mod == null ? HttpStatusCode.NotFound : HttpStatusCode.OK);
        }

        public virtual async Task<TModel> UpdateModelAsync(int id, TModel item)
        {
            var mod = await this.ModelSet.SingleOrDefaultAsync(c => c.Id == id);
            if (mod != null)
            {
                mod.Copy(item);
                mod.Id = id;
                await this.SaveChangesAsync();
            }
            return mod;
        }
    }
}
