using Common.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Common.Interfaces
{
    public interface IExceptionHandler
    {
        Task<IActionResult> SendResponse(ControllerBase controller, Task<MarsResponse> response);
    }
}
