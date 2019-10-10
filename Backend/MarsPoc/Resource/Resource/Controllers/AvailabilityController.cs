using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Common.Interfaces;
using Common.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Resource.Models;
using Resource.Repositories;

namespace Resource.Controllers
{
    [Route("api/resource/availability")]
    [ApiController]
    public class AvailabilityController : ControllerBase
    {

        private readonly AvailabilityRepository availabilitycontext;
        private readonly ResourceRepository resourceContext;
        private readonly ILogHandler logHandler;
        private readonly IExceptionHandler exceptionHandler;

        public AvailabilityController(AvailabilityRepository availabilitycontext, ResourceRepository resourceContext, ILogHandler logHandler, IExceptionHandler exceptionHandler)
        {
            this.availabilitycontext = availabilitycontext;
            this.resourceContext = resourceContext;
            this.logHandler = logHandler;
            this.exceptionHandler = exceptionHandler;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpPost()]
        [ProducesResponseType(typeof(MarsResponse), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(MarsResponse), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> GetAvailabilty([FromBody] AvailabilityModel objAvailabilityModel)
        {
            var lstavilabity =  await this.availabilitycontext.GetAsync(objAvailabilityModel);
            var res = (from avilabity in lstavilabity
                       join resource in resourceContext.ModelSet
                       on avilabity.ResourceId equals resource.Id
                       select new {
                           ResourceId = resource.Id,
                           Name = resource.ResourceName,
                           Date = avilabity.Date,
                           StartTime = avilabity.StartTime,
                           EndTime = avilabity.EndTime                           
                       }).ToArray();

            HttpStatusCode code = HttpStatusCode.OK;
            if (res == null)
                code = HttpStatusCode.NotFound;
            var resMars =  Task.Run(()=>MarsResponse.GetResponse(res, code));
            return await this.exceptionHandler.SendResponse(this, resMars);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        // GET: api/resource/availability/
        [HttpGet]
        [ProducesResponseType(typeof(MarsResponse), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(MarsResponse), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> GetAllvailabilty()
        {
            var lstavilabity = await this.availabilitycontext.GetAllAsync(DateTime.Now.Date);
            var res = (from avilabity in lstavilabity
                       join resource in resourceContext.ModelSet
                       on avilabity.ResourceId equals resource.Id
                       select new
                       {
                           Id = resource.Id,
                           ResourceId = resource.Id,
                           Name = resource.ResourceName,
                           Date = avilabity.Date,
                           StartTime = avilabity.StartTime,
                           EndTime = avilabity.EndTime
                       }).ToArray();

            HttpStatusCode code = HttpStatusCode.OK;
            if (res == null)
                code = HttpStatusCode.NotFound;
            var resMars = Task.Run(() => MarsResponse.GetResponse(res, code));
            return await this.exceptionHandler.SendResponse(this, resMars);
        }



        //resource_type, date : find all the available resources of given type on given date
        //[Route("resources")]
        //[HttpPost()]
        //[ProducesResponseType(typeof(MarsResponse), (int)HttpStatusCode.Created)]
        //[ProducesResponseType(typeof(MarsResponse), (int)HttpStatusCode.InternalServerError)]
        //public async Task<IActionResult> Post(ResourcePostTypeViewModel objResponseTypeViewModel)
        //{
        //    context.GEt
        //}
    }
}