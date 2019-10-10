using Authentication.Models;
using Common.Base;
using Common.Interfaces;
using Common.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Authentication.Repositories
{
    public class ImageHandlerRepository : RepositoryBase<ImageFileStoreModel, ImageHandlerRepository>
    {
        private readonly ILogHandler logHandler;

        public ImageHandlerRepository(DbContextOptions<ImageHandlerRepository> options, ILogHandler logHandler) : base(options, logHandler)
        {

        }

        public async Task<ImageFileStoreModel> GetModelAsync(Guid id)
        {
            return await this.ModelSet.FirstOrDefaultAsync(m => m.GuId == id);
        }


        //public async Task<MarsResponse> GetModelAsync(Guid id)
        //{
        //    var res = await this.ModelSet.FirstOrDefaultAsync(m => m.GuId == id);
        //    HttpStatusCode code = HttpStatusCode.OK;
        //    if (res == null)
        //        code = HttpStatusCode.NotFound;
        //    return MarsResponse.GetResponse(res, code);
        //}

        public async Task<MarsResponse> AddImagModel(ImageFileStoreModel item)
        {
            return await this.CreateAsync(item);
        }

        protected override IEnumerable<ImageFileStoreModel> FilterSearch(ImageFileStoreModel item)
        {
            IEnumerable<ImageFileStoreModel> imageFileStore = null;

            imageFileStore = this.ModelSet.Where(c => c.GuId == item.GuId).ToArray();

            return imageFileStore;
        }



    }
}
