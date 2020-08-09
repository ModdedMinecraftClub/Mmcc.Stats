using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Mmcc.Stats.Core.Interfaces;

namespace Mmcc.Stats.Infrastructure.Authentication
{
    public class ClientAppAuthenticationOptions : AuthenticationSchemeOptions
    {
    }
    
    public class ClientAppAuthenticationHandler : AuthenticationHandler<ClientAppAuthenticationOptions>
    {
        private readonly ITokenService _tokenService;

        public ClientAppAuthenticationHandler(
            IOptionsMonitor<ClientAppAuthenticationOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock,
            ITokenService tokenService
            ) 
            : base(options, logger, encoder, clock)
        {
            _tokenService = tokenService;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.ContainsKey("X-Auth-Token"))
            {
                return AuthenticateResult.Fail("Unauthorized");
            }

            var tokenHeader = Request.Headers["X-Auth-Token"];

            if (string.IsNullOrEmpty(tokenHeader))
            {
                return AuthenticateResult.Fail("Unauthorized");
            }

            try
            {
                return await ValidateToken(tokenHeader);
            }
            catch (Exception e)
            {
                Logger.LogError(e, "Error while authenticating.");
                return AuthenticateResult.Fail("Internal Server Error");
            }
        }

        private async Task<AuthenticateResult> ValidateToken(string token)
        {
            var validatedToken = await _tokenService.GetToken(token);
            
            if (validatedToken == null)
            {
                return AuthenticateResult.Fail("Unauthorized");
            }
            
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, validatedToken.ClientName),
            };
            var identity = new ClaimsIdentity(claims, Scheme.Name);
            var principal = new System.Security.Principal.GenericPrincipal(identity, null);
            var ticket = new AuthenticationTicket(principal, Scheme.Name);
            return AuthenticateResult.Success(ticket);
        }
    }
}