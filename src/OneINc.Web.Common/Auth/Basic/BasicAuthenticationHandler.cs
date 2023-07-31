using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OneINc.Web.Common.Models;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;

namespace OneINc.Web.Common.Auth.Basic
{
    public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private readonly BasicAuthOptions _settings;
        public BasicAuthenticationHandler
            (IOptions<BasicAuthOptions> configs, IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock) : base(options, logger, encoder, clock)
        {
            _settings = configs.Value ?? throw new ArgumentNullException(nameof(configs)); 
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var endpoint = Request.HttpContext.GetEndpoint();
            
            if (endpoint?.Metadata.GetMetadata<IAllowAnonymous>() !=null) 
            {
                return Task.FromResult(AuthenticateResult.Success(GetAuthenticationTicket("temp")));
            }
            else 
            {

                // No authorization header, so throw no result.
                if (!Request.Headers.ContainsKey("Authorization"))
                {
                    return Task.FromResult(AuthenticateResult.Fail("Missing Authorization header"));
                }
                var authorizationHeader = Request.Headers["Authorization"].ToString();

                // If authorization header doesn't start with basic, throw no result.
                if (!authorizationHeader.StartsWith("Basic ", StringComparison.OrdinalIgnoreCase))
                {
                    return Task.FromResult(AuthenticateResult.Fail("Authorization header does not start with 'Basic'"));
                }

                // Decrypt the authorization header and split out the client id/secret which is separated by the first ':'
                var authBase64Decoded = Encoding.UTF8.GetString(Convert.FromBase64String(authorizationHeader.Replace("Basic ", "", StringComparison.OrdinalIgnoreCase)));
                var authSplit = authBase64Decoded.Split(new[] { ':' }, 2);

                // No username and password, so throw no result.
                if (authSplit.Length != 2)
                {
                    return Task.FromResult(AuthenticateResult.Fail("Invalid Authorization header format"));
                }

                // Store the client ID and secret
                var clientId = authSplit[0];
                var clientSecret = authSplit[1];

                // Client ID and secret are incorrect
                if (clientId != _settings.NameValue || clientSecret != _settings.PasswordValue)
                {
                    return Task.FromResult(AuthenticateResult.Fail(string.Format("The secret is incorrect for the client '{0}'", clientId)));
                }


                // Return a success result.
                return Task.FromResult(AuthenticateResult.Success(GetAuthenticationTicket(clientId)));
            }
        }

        private AuthenticationTicket GetAuthenticationTicket(string clientId) 
        {
            // Authenicate the client using basic authentication
            var client = new BasicAuthenticationClient
            {
                AuthenticationType = BasicAuthenticationDefaults.AuthenticationScheme,
                IsAuthenticated = true,
                Name = clientId
            };

            // Set the client ID as the name claim type.
            ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(client, new[]
            {
                new Claim(ClaimTypes.Name, clientId)
            }));

            return new AuthenticationTicket(claimsPrincipal, Scheme.Name);
        }
    }
}
