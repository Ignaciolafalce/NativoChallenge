﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NativoChallenge.Application.Interfaces
{
    public interface ICurrentUserService
    {
        string? UserId { get; }
        string? UserName { get; }
        bool IsInRole(string roleName);
    }
}
