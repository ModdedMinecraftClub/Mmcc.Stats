using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Mmcc.Stats.Core.Data;

namespace Mmcc.Stats.Infrastructure.Authentication.ClientAppAuthentication
{
    public class ClientAppAuthenticationOptions : AuthenticationSchemeOptions
    {
    }
    
    public class ClientAppAuthenticationHandler : AuthenticationHandler<ClientAppAuthenticationOptions>
    {
        private readonly IMediator _mediator;

        public ClientAppAuthenticationHandler(
            IOptionsMonitor<ClientAppAuthenticationOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock,
            IMediator mediator
            ) 
            : base(options, logger, encoder, clock)
        {
            _mediator = mediator;
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
            var validatedToken = (await _mediator.Send(new ValidateTokenViaDb.Query {IncomingTokenValue = token}))
                .ValidatedToken;
            
            if (validatedToken == null) return AuthenticateResult.Fail("Unauthorized");
            
            var claims = new List<Claim> {new Claim(ClaimTypes.Name, validatedToken.ClientName)};
            var identity = new ClaimsIdentity(claims, Scheme.Name);
            var principal = new System.Security.Principal.GenericPrincipal(identity, null);
            var ticket = new AuthenticationTicket(principal, Scheme.Name);
            return AuthenticateResult.Success(ticket);
        }
    }
}