﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Library.API.AuthExtensions
{
    public interface IAuthenticationExtension
    {
        string GetUserName();
        string GetUserId(string userName);
    }
}
