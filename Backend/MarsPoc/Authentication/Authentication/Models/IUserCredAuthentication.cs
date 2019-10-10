﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Authentication.Models
{
    public interface IUserCredAuthentication
    {
        Task<AuthDbModel> Authenticate(string userName, string Password);
    }
}
