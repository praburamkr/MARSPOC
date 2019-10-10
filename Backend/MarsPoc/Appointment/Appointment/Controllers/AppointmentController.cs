using Appointment.Models;
using Appointment.Repositories;
using Common.Interfaces;
using Common.Models;
using Common.Notification;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;
using System.Threading.Tasks;

namespace Appointment.Controllers
{
    [Route("api/appointments")]
    [ApiController]
    public class AppointmentController : ControllerBase
    {
        private readonly AppointmentRepository context;
        private readonly AppointmentTypeRepository appointmentTypeContext;
        private readonly ReasonCodeRepository reasonCodeContext;
        private readonly IExceptionHandler exceptionHandler;
        private readonly IAvailabilitySearch availabilitySearchContext;

        public AppointmentController(
            AppointmentRepository context,
            AppointmentTypeRepository appointmentTypeContext,
            ReasonCodeRepository reasonCodeContext,
            IAvailabilitySearch availabilityContext,
            IExceptionHandler exceptionHandler)
        {
            this.context = context;
            this.appointmentTypeContext = appointmentTypeContext;
            this.reasonCodeContext = reasonCodeContext;
            this.availabilitySearchContext = availabilityContext;
            this.exceptionHandler = exceptionHandler;
        }

        // POST: api/appointments
        [HttpPost]
        [ProducesResponseType(typeof(MarsResponse), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(MarsResponse), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> CreateAppointments([FromBody] object jsonObj)
        {
            var token = HttpContext.Request.Headers["Authentication"];
            return await this.exceptionHandler.SendResponse(this, this.context.CreateAllAsync(jsonObj, token));
        }

        // GET: api/appointments/5
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(MarsResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(MarsResponse), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(MarsResponse), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> GetAppointmentDetails(int id)
        {
            var token = HttpContext.Request.Headers["Authentication"];
            return await this.exceptionHandler.SendResponse(this, this.context.GetAppAsync(id, token));
        }

        // POST: api/appointments/search
        [HttpPost("search")]
        [ProducesResponseType(typeof(MarsResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(MarsResponse), (int)HttpStatusCode.NotAcceptable)]
        [ProducesResponseType(typeof(MarsResponse), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(MarsResponse), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> SearchAppointments([FromBody] object jsonObj)
        {
            var token = HttpContext.Request.Headers["Authentication"];
            return await this.exceptionHandler.SendResponse(this, this.context.SearchAppointmentsAsync(jsonObj, token));
        }

        // DELETE: api/appointments/5
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(MarsResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(MarsResponse), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(MarsResponse), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> DeleteAppointment(int id)
        {
            return await this.exceptionHandler.SendResponse(this, this.context.DeleteAsync(id));
        }

        // PUT: api/appointments/5
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(MarsResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(MarsResponse), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(MarsResponse), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> UpdateAppointment(int id, [FromBody] AppointmentModel appointment)
        {
            return await this.exceptionHandler.SendResponse(this, this.context.UpdateAsync(id, appointment));
        }

        // POST: api/appointments/checkin
        [HttpPost("checkin")]
        [ProducesResponseType(typeof(MarsResponse), (int)HttpStatusCode.OK)]        
        [ProducesResponseType(typeof(MarsResponse), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(MarsResponse), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> SetAppointmentCheckIn([FromBody] AppointmentModel appointment)
        {
            appointment.Status = Convert.ToInt32(AppointmentStatus.CheckedIn);
            appointment.WorkflowState = Convert.ToInt32(AppointmentStatus.CheckedIn);
            var token = HttpContext.Request.Headers["Authentication"];
            return await this.exceptionHandler.SendResponse(this, this.context.UpdateStatusWithNotification(appointment, NotificationEvent.CHECKIN, token));
        }

        // POST: api/appointments/checkout
        [HttpPost("checkout")]
        [ProducesResponseType(typeof(MarsResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(MarsResponse), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(MarsResponse), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> SetAppointmentCheckOut([FromBody] AppointmentModel appointment)
        {
            appointment.Status = Convert.ToInt32(AppointmentStatus.CheckedOut);
            appointment.WorkflowState = Convert.ToInt32(AppointmentStatus.CheckedOut);
            return await this.exceptionHandler.SendResponse(this, this.context.UpdateStatus(appointment));
        }

        // POST: api/appointments/workflow/begin
        [HttpPost("workflow/begin")]
        [ProducesResponseType(typeof(MarsResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(MarsResponse), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(MarsResponse), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> SetAppointmentBegin([FromBody] AppointmentModel appointment)
        {
            appointment.Status = Convert.ToInt32(AppointmentStatus.Confirmed);
            appointment.WorkflowState = Convert.ToInt32(AppointmentStatus.Confirmed);
            return await this.exceptionHandler.SendResponse(this, this.context.UpdateStatus(appointment));
        }

        // POST: api/appointments/workflow/claim
        [HttpPost("workflow/claim")]
        [ProducesResponseType(typeof(MarsResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(MarsResponse), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(MarsResponse), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> SetAppointmentClaim([FromBody] AppointmentModel appointment)
        {
            appointment.Status = Convert.ToInt32(AppointmentStatus.InProgress);
            appointment.WorkflowState = Convert.ToInt32(AppointmentStatus.InProgress);
            var token = HttpContext.Request.Headers["Authentication"];
            return await this.exceptionHandler.SendResponse(this, this.context.UpdateStatusWithNotification(appointment, NotificationEvent.CLAIM, token));
        }

        // POST: api/appointments/workflow/respond
        [HttpPost("workflow/respond")]
        [ProducesResponseType(typeof(MarsResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(MarsResponse), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(MarsResponse), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> SetAppointmentRespond([FromBody] AppointmentModel appointment)
        {
            appointment.Status = Convert.ToInt32(AppointmentStatus.CheckedIn);
            appointment.WorkflowState = Convert.ToInt32(AppointmentStatus.CheckedIn);
            var token = HttpContext.Request.Headers["Authentication"];
            return await this.exceptionHandler.SendResponse(this, this.context.UpdateStatusWithNotification(appointment, NotificationEvent.RESPOND, token));
        }

        // GET: api/appointments/types
        [HttpGet("types")]
        [ProducesResponseType(typeof(MarsResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(MarsResponse), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(MarsResponse), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> GetAppointmentTypes()
        {
            return await this.exceptionHandler.SendResponse(this, this.appointmentTypeContext.SearchAsync(null));
        }

        // GET: api/appointments/types/2
        [HttpGet("types/{id}")]
        [ProducesResponseType(typeof(MarsResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(MarsResponse), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(MarsResponse), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> GetParentAppointmentTypes(int id)
        {
            return await this.exceptionHandler.SendResponse(this, this.appointmentTypeContext.GetParentModelsAsync(id));
        }

        // GET: api/appointments/reasons/codes
        [HttpGet("reasons/codes")]
        [ProducesResponseType(typeof(MarsResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(MarsResponse), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(MarsResponse), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> GetReasonCodes()
        {
            return await this.exceptionHandler.SendResponse(this, this.reasonCodeContext.SearchAsync(null));
        }

        // POST: api/appointments/timeslots/search				
        [HttpPost("timeslots/search")]
        [ProducesResponseType(typeof(MarsResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(MarsResponse), (int)HttpStatusCode.NotAcceptable)]
        [ProducesResponseType(typeof(MarsResponse), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(MarsResponse), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> SearchAvailabilityOfResource([FromBody] AvailabilitySerachModel availabilitySearch)
        {
            var token = HttpContext.Request.Headers["Authentication"];
            return await this.exceptionHandler.SendResponse(this, this.availabilitySearchContext.SearchAvailabilityAsync(availabilitySearch, token));
        }

    }
}