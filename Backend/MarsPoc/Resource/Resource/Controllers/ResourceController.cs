using Common.Interfaces;
using Common.Models;
using Microsoft.AspNetCore.Mvc;
using Resource.Models;
using Resource.Repositories;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace Resource.Controllers
{
    [Route("api/resources")]
    [ApiController]
    public class ResourceController : ControllerBase
    {
        private readonly ResourceRepository context;
        private readonly ILogHandler logHandler;
        private readonly IExceptionHandler exceptionHandler;

        public ResourceController(ResourceRepository context, ILogHandler logHandler, IExceptionHandler exceptionHandler)
        {
            this.context = context;
            this.logHandler = logHandler;
            this.exceptionHandler = exceptionHandler;
        }

        // POST: api/resources
        [HttpPost]
        [ProducesResponseType(typeof(MarsResponse), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(MarsResponse), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> CreateResource([FromBody] ResourceModel resource)
        {
            return await this.exceptionHandler.SendResponse(this, this.context.CreateAsync(resource));
        }

        // GET: resources/5
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(MarsResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(MarsResponse), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(MarsResponse), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> GetResourceDetails(int id)
        {
            return await this.exceptionHandler.SendResponse(this, this.context.GetAsync(id));
        }

        // POST: resources/search
        [HttpPost("search")]
        [ProducesResponseType(typeof(MarsResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(MarsResponse), (int)HttpStatusCode.NotAcceptable)]
        [ProducesResponseType(typeof(MarsResponse), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(MarsResponse), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> SearchResources([FromBody] ResourceModel resource)
        {
            return await this.exceptionHandler.SendResponse(this, this.context.SearchAsync(resource));
        }

        // DELETE: resources/5
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(MarsResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(MarsResponse), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(MarsResponse), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> DeleteResource(int id)
        {
            return await this.exceptionHandler.SendResponse(this, this.context.DeleteAsync(id));
        }

        // PUT: resources/5
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(MarsResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(MarsResponse), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(MarsResponse), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> UpdateResource(int id, [FromBody] ResourceModel resource)
        {
            return await this.exceptionHandler.SendResponse(this, this.context.UpdateAsync(id, resource));
        }

        // POST: resources/searchall
        [HttpPost("searchall")]
        [ProducesResponseType(typeof(MarsResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(MarsResponse), (int)HttpStatusCode.NotAcceptable)]
        [ProducesResponseType(typeof(MarsResponse), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(MarsResponse), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> SearchAllResourceNames([FromBody] IEnumerable<int> idList)
        {
            return await this.exceptionHandler.SendResponse(this, this.context.SearchAllAsync(idList));
        }
    }
}
