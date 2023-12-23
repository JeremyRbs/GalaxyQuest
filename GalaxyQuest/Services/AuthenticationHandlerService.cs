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
    /// <summary>
    /// Custom authentication handler for handling basic authentication.
    /// </summary>
    public class AuthenticationHandlerService : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AuthenticationHandlerService"/> class.
        /// </summary>
        /// <param name="options">The options for the authentication scheme.</param>
        /// <param name="logger">The logger for this authentication handler.</param>
        /// <param name="encoder">The URL encoder.</param>
        /// <param name="clock">The system clock.</param>
        public AuthenticationHandlerService(
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock) : base(options, logger, encoder, clock) { }

        /// <summary>
        /// Handles the authentication process asynchronously.
        /// </summary>
        /// <returns>The result of the authentication process.</returns>
        protected async override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            // Check if the Authorization header is present in the request
            if (!Request.Headers.ContainsKey("Authorization"))
            {
                return AuthenticateResult.NoResult();
            }

            // Extract the credentials from the Authorization header
            string authHeader = Request.Headers["Authorization"].ToString().Substring(6);
            string decodedString = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(authHeader));

            // Validate the credentials and determine if the user is an admin
            bool isAdmin = IsValidCredentials(decodedString);

            // Create claims for the authenticated user
            var claims = new[] {
                new Claim(ClaimTypes.Name, isAdmin ? "admin" : "fake-remote"),
                new Claim(ClaimTypes.Role, isAdmin ? "admin" : "shard")
            };
            var principal = new ClaimsPrincipal(new ClaimsIdentity(claims, "Tokens"));
            var ticket = new AuthenticationTicket(principal, Scheme.Name);

            // Return a successful authentication result
            return AuthenticateResult.Success(ticket);
        }

        /// <summary>
        /// Validates the provided credentials.
        /// </summary>
        /// <param name="decodedString">The decoded string containing credentials.</param>
        /// <returns>True if the credentials are valid; otherwise, false.</returns>
        private bool IsValidCredentials(string decodedString)
        {
            // Simple validation logic for demonstration purposes
            return decodedString.Contains("admin:password");
        }
    }
}
