using Microsoft.Extensions.Options;
using OneINc.Web.Common.Models;
using OneINc.Web.Common.Models.Requests;
using OneINc.Web.Common.Models.Responses;
using OneINc.Web.Core.Interfaces;

namespace OneINc.Web.Core.Services
{
    /// <summary>
    /// Authentication service 
    /// </summary>
    public class AuthService : IAuthService
    {
        private readonly BasicAuthOptions _settings;
        public AuthService(IOptions<BasicAuthOptions> configs)
        { 
            _settings = configs.Value ?? throw new ArgumentNullException(nameof(configs));  
        }

        /// <inheritdoc/>
        public async Task<AuthLoginResponse> LoginAsync(AuthLoginRequest request) 
        {
            var retVal = false;

            if (request?.Password == _settings.PasswordValue && request?.Username == _settings.NameValue) 
            {
                retVal = true;
            }

            return new AuthLoginResponse(retVal);
        }
    }
}
