using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;

namespace GalaxyQuest.Services
{
    /// <summary>
    /// Custom authentication handler for handling basic authentication.
    /// </summary>
    public class AuthenticationHandlerService : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AuthenticationHandlerService"/> class.
        /// </summary>
        public AuthenticationHandlerService(
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock)
            : base(options, logger, encoder, clock)
        {
        }

        /// <summary>
        /// Handles the authentication process.
        /// </summary>
        /// <returns>The result of the authentication process.</returns>
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
                        var claims = new[] { new Claim(ClaimTypes.Name, "Username") };
                        var identity = new ClaimsIdentity(claims, Scheme.Name);
                        var principal = new ClaimsPrincipal(identity);
                        var ticket = new AuthenticationTicket(principal, Scheme.Name);
                        return AuthenticateResult.Success(ticket);
                    }
                    else
                    {
                        return AuthenticateResult.Fail("Invalid credentials.");
                    }
                }
            }
            return AuthenticateResult.NoResult();
        }

        /// <summary>
        /// Validates the provided credentials.
        /// </summary>
        /// <param name="decodedCredentials">The decoded credentials.</param>
        /// <returns>True if the credentials are valid; otherwise, false.</returns>
        private bool IsValidCredentials(string decodedCredentials)
        {
            var splitted = decodedCredentials.Split(':');
            return splitted.Length == 2 && splitted[0] == "admin" && splitted[1] == "password";
        }
    }
}
