using System;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace GalaxyQuest.Services
{
    public class AuthenticationHandlerService : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        public AuthenticationHandlerService(
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock)
            : base(options, logger, encoder, clock)
        {
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (Request.Headers.ContainsKey("Authorization"))
            {
                var authorizationHeader = Request.Headers["Authorization"].ToString();

                if (authorizationHeader.StartsWith("Basic "))
                {
                    var base64Credentials = authorizationHeader.Substring("Basic ".Length).Trim();

                    var decodedCredentials = Encoding.UTF8.GetString(Convert.FromBase64String(base64Credentials));

                    if (IsValidCredentials(decodedCredentials))
                    {
                        var claims = new[] { new Claim(ClaimTypes.Name, "TonUsername") };
                        var identity = new ClaimsIdentity(claims, Scheme.Name);
                        var principal = new ClaimsPrincipal(identity);
                        var ticket = new AuthenticationTicket(principal, Scheme.Name);
                        return AuthenticateResult.Success(ticket);
                    }
                    else
                    {
                        return AuthenticateResult.Fail("Informations d'identification invalides");
                    }
                }
            }
            return AuthenticateResult.NoResult();
        }

        private bool IsValidCredentials(string decodedCredentials)
        {
            var splitted = decodedCredentials.Split(':');
            return splitted.Length == 2 && splitted[0] == "admin" && splitted[1] == "password";
        }
    }
}
