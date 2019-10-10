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
    [Route("api/patients")]
    [ApiController]
    public class PatientController : ControllerBase
    {
        private readonly PatientRepository context;
        private readonly ILogHandler logHandler;
        private readonly IExceptionHandler exceptionHandler;

        public PatientController(PatientRepository context, ILogHandler logHandler, IExceptionHandler exceptionHandler)
        {
            this.context = context;
            this.logHandler = logHandler;
            this.exceptionHandler = exceptionHandler;
        }

        // POST: api/patients
        [HttpPost]
        [ProducesResponseType(typeof(MarsResponse), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(MarsResponse), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> CreatePatient([FromBody] PatientModel patient)
        {
            return await this.exceptionHandler.SendResponse(this, this.context.CreateAsync(patient));
        }

        // GET: api/patients/5
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(MarsResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(MarsResponse), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(MarsResponse), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> GetPatientDetails(int id)
        {
            return await this.exceptionHandler.SendResponse(this, this.context.GetAsync(id));
        }

        // POST: api/patients/search
        [HttpPost("search")]
        [ProducesResponseType(typeof(MarsResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(MarsResponse), (int)HttpStatusCode.NotAcceptable)]
        [ProducesResponseType(typeof(MarsResponse), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(MarsResponse), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> SearchPatients([FromBody] PatientModel patient)
        {
            return await this.exceptionHandler.SendResponse(this, this.context.SearchAsync(patient));
        }

        [HttpPost("searchall")]
        [ProducesResponseType(typeof(MarsResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(MarsResponse), (int)HttpStatusCode.NotAcceptable)]
        [ProducesResponseType(typeof(MarsResponse), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(MarsResponse), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> SearchAllPatients([FromBody] IEnumerable<int> idList)
        {
            return await this.exceptionHandler.SendResponse(this, this.context.SearchAllAsync(idList));
        }

        // DELETE: api/patients/5
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(MarsResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(MarsResponse), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(MarsResponse), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> DeletePatient(int id)
        {
            return await this.exceptionHandler.SendResponse(this, this.context.DeleteAsync(id));
        }

        // PUT: api/patients/5
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(MarsResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(MarsResponse), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(MarsResponse), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> UpdatePatient(int id, [FromBody] PatientModel patient)
        {
            return await this.exceptionHandler.SendResponse(this, this.context.UpdateAsync(id, patient));
        }
    }
}