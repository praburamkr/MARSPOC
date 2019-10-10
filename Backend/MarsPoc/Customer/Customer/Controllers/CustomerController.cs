using Common.Interfaces;
using Common.Models;
using Customer.Models;
using Customer.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace Customer.Controllers
{
    [Route("api/clients")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly CustomerRepository context;
        private readonly ILogHandler logHandler;
        private readonly IExceptionHandler exceptionHandler;

        public CustomerController(CustomerRepository context, ILogHandler logHandler, IExceptionHandler exceptionHandler)
        {
            this.context = context;
            this.logHandler = logHandler;
            this.exceptionHandler = exceptionHandler;
        }

        // POST: api/clients
        [HttpPost]
        [ProducesResponseType(typeof(MarsResponse), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(MarsResponse), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> CreateCustomer([FromBody] CustomerModel customer)
        {
            return await this.exceptionHandler.SendResponse(this, this.context.CreateAsync(customer));
        }

        // GET: clients/5
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(MarsResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(MarsResponse), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(MarsResponse), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> GetCustomerDetails(int id)
        {
            return await this.exceptionHandler.SendResponse(this, this.context.GetAsync(id));
        }

        // POST: clients/search
        [HttpPost("search")]
        [ProducesResponseType(typeof(MarsResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(MarsResponse), (int)HttpStatusCode.NotAcceptable)]
        [ProducesResponseType(typeof(MarsResponse), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(MarsResponse), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> SearchCustomers([FromBody] CustomerModel customer)
        {
            return await this.exceptionHandler.SendResponse(this, this.context.SearchAsync(customer));
        }

        // GET: clients/searchall
        [HttpGet("searchall")]
        [ProducesResponseType(typeof(MarsResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(MarsResponse), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(MarsResponse), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> GetAllCustomers()
        {
            return await this.exceptionHandler.SendResponse(this, this.context.GetAllCustomersAsync());
        }

        // POST: clients/searchall
        [HttpPost("searchall")]
        [ProducesResponseType(typeof(MarsResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(MarsResponse), (int)HttpStatusCode.NotAcceptable)]
        [ProducesResponseType(typeof(MarsResponse), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(MarsResponse), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> SearchAllCustomers([FromBody] IEnumerable<int> idList)
        {
            return await this.exceptionHandler.SendResponse(this, this.context.SearchAllAsync(idList));
        }

        // DELETE: clients/5
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(MarsResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(MarsResponse), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(MarsResponse), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> DeleteCustomer(int id)
        {
            return await this.exceptionHandler.SendResponse(this, this.context.DeleteAsync(id));
        }

        // PUT: clients/5
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(MarsResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(MarsResponse), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(MarsResponse), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> UpdateCustomer(int id, [FromBody] CustomerModel customer)
        {
            return await this.exceptionHandler.SendResponse(this, this.context.UpdateAsync(id, customer));
        }
    }
}
