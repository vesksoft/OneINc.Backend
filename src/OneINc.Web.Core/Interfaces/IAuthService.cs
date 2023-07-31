using OneINc.Web.Common.Models.Requests;
using OneINc.Web.Common.Models.Responses;

namespace OneINc.Web.Core.Interfaces
{
    public interface IAuthService
    {
        /// <summary>
        /// Method to check login creds
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<AuthLoginResponse> LoginAsync(AuthLoginRequest request);
    }
}
