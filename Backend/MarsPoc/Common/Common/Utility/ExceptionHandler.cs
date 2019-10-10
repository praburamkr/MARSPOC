using Common.Interfaces;
using Common.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Common.Utility
{
    public class ExceptionHandler : IExceptionHandler
    {
        private readonly ILogHandler log;

        public ExceptionHandler(ILogHandler log)
        {
            this.log = log;            
        }

        public async Task<IActionResult> SendResponse(ControllerBase controller, Task<MarsResponse> response)
        {
            try
            {
                var resp = await response;
                return controller.StatusCode((int)resp.Code, resp);
            }
            catch (Exception e)
            {
                this.log.LogError(e);
                return controller.StatusCode(500, MarsResponse.GetError(e));
            }
        }
    }
}
