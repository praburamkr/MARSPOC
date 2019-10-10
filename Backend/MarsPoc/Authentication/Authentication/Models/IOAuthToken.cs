using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Authentication.Models
{
    public interface IOAuthToken
    {
        Task<ResponseToken> GetToken(string userName, string password);
    }
}
