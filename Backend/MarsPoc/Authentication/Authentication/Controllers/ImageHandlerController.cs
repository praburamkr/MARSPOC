using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Authentication.Models;
using Authentication.Repositories;
using Common.Interfaces;
using Common.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.WindowsAzure.Storage.Blob;

namespace Authentication.Controllers
{
    [Route("api/images")]
    [ApiController]
    public class ImageHandlerController : ControllerBase
    {
        private readonly ImageHandlerRepository context;
        private readonly ILogHandler logHandler;
        private readonly IExceptionHandler exceptionHandler;

        public ImageHandlerController(ImageHandlerRepository context, ILogHandler logHandler, IExceptionHandler exceptionHandler)
        {
            this.context = context;
            this.logHandler = logHandler;
            this.exceptionHandler = exceptionHandler;
        }


        /// <summary>
        /// 1)The Api Expose to Upload any of the file to blob storage 
        /// 2)get the Url from blob storage 
        /// 3)Store the Url in the table by generating a Guid
        /// </summary>
        /// <param name="file">request body</param>
        /// <returns></returns>
        [Route("upload")]
        [HttpPost]
        [ProducesResponseType(typeof(MarsResponse), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(MarsResponse), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> Image([FromForm] IFormFile file)
        {
            //Store the Image stream on to the storage 
            var stream = file.OpenReadStream();
            BlobStorageService blobStorageService = new BlobStorageService();
            var blobUrl = await blobStorageService.UploadFileToStorage(stream, file.FileName);
            //Insert the data on to the table


            //Return the Guid create for the ImageHandler table
            ImageFileStoreModel objImageFileStoreModel = new ImageFileStoreModel();
            objImageFileStoreModel.Url = blobUrl; 
            ////await this.context();
            //Task<MarsResponse> marsResponse = null;

            return await this.exceptionHandler.SendResponse(this, this.context.AddImagModel(objImageFileStoreModel));
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="Id">Send the Guid to get the data with Url</param>
        /// <returns></returns>
        [Route("download/{Id}")]
        [ProducesResponseType(typeof(MarsResponse), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(MarsResponse), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> GetImage(Guid Id)
        {
            MemoryStream ms = new MemoryStream();

            var response = await this.context.GetModelAsync(Id);

            BlobStorageService blobStorageService = new BlobStorageService();
            var container = blobStorageService.GetContainerFromStorage();

            if (await container.ExistsAsync())
            {
                CloudBlob file = container.GetBlobReference(response.Url);

                if (await file.ExistsAsync())
                {
                    await file.DownloadToStreamAsync(ms);
                    Stream blobStream = file.OpenReadAsync().Result;
                    return File(blobStream, file.Properties.ContentType, file.Name);
                }
                else
                {
                    return Content("File does not exist");
                }
            }
            else
            {
                return Content("Container does not exist");
            }
        }
    }
}
