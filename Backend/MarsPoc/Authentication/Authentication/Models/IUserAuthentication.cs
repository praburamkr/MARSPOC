using Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Authentication.Models
{
    public interface IUserAuthentication
    {
        Task<MarsResponse> SignIn(string userName, string password);
        Task<string> TokenAuthentication(string token);
    }
}
