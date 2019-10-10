using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Authentication.Models
{
    public interface IJWTToken
    {
       string GetToken(string userId);
    }
}
