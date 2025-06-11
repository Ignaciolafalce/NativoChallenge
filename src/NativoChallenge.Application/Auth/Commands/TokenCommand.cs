using MediatR;
using NativoChallenge.Application.Auth.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NativoChallenge.Application.Auth.Commands
{
    public class TokenCommand : IRequest<TokenResult>
    {
        public string UserName { get; set; } = default!;
        public string Password { get; set; } = default!;
    }

}
