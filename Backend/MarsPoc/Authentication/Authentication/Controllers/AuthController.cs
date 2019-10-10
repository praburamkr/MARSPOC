using Authentication.Models;
using Common.Interfaces;
using Common.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Authentication.Controllers
{
    [Route("api/security")]
    [ApiController]
    [EnableCors("AllowAnyOrigin")]
    public class AuthController : ControllerBase
    {
        private readonly IUserAuthentication authentication;
        private readonly ILogHandler logHandler;
        private readonly IExceptionHandler exceptionHandler;

        public AuthController(IUserAuthentication auth, ILogHandler logHandler, IExceptionHandler exceptionHandler)
        {
            authentication = auth;
            this.logHandler = logHandler;
            this.exceptionHandler = exceptionHandler;
        }

        [HttpPost]
        [Route("login")]
        [ProducesResponseType(typeof(MarsResponse), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(MarsResponse), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> SignIn([FromBody] UserCredentialsModel user)
        {
            string userName = user.UserName;
            string password = user.Password;

            return await this.exceptionHandler.SendResponse(this, authentication.SignIn(userName, password));
        }

        [HttpGet("{token}")]
        [Route("token/find")]
        public async Task<string> ValidateToken(string token)
        {
            var result = await authentication.TokenAuthentication(token);

            return result;
        }
    }
}
