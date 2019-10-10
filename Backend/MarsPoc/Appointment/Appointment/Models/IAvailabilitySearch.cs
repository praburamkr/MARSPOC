using Common.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Appointment.Models
{
    public interface IAvailabilitySearch
    {
        Task<IActionResult> SendResponse(ControllerBase controller, Task<MarsResponse> response);

        Task<MarsResponse> SearchAvailabilityAsync(AvailabilitySerachModel availabilitySearch, string token);
    }
}
