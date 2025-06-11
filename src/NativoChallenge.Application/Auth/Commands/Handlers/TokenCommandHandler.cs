using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using NativoChallenge.Application.Auth.DTOs;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using NativoChallenge.Application.Common.Exceptions;
using NativoChallenge.Domain.Entities.Identity;

namespace NativoChallenge.Application.Auth.Commands.Handlers
{
    public class TokenCommandHandler : IRequestHandler<TokenCommand, TokenResult>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;

        public TokenCommandHandler(UserManager<ApplicationUser> userManager, IConfiguration configuration)
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public async Task<TokenResult> Handle(TokenCommand request, CancellationToken cancellationToken)
        {
            // Verify user credentials
            var user = await _userManager.FindByNameAsync(request.UserName);

            if (user is null || !await _userManager.CheckPasswordAsync(user, request.Password))
            {
                throw new ForbiddenAccessException("Forbidden Access"); // i need to create api exceptions 
            }

            var roles = await _userManager.GetRolesAsync(user);

            // Create token Claims
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Sid, user.Id.ToString()),
                //new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, user.UserName!)
            };

            // Add roles to token Claims
            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
            var credential = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var tokenDescriptor = new JwtSecurityToken(
                    issuer: _configuration["Jwt:Issuer"],
                    audience: _configuration["Jwt:Audience"],
                    claims: claims,
                    expires: DateTime.UtcNow.AddMinutes(420), // Set expiration time
                    signingCredentials: credential);

            var jwt = new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);

            // Cambiar return !
            return new TokenResult
            {
                AccessToken = jwt
            };

        }
    }

}
