using One.INc.Web.API.Endpoints.v1.Auth;
using One.INc.Web.API.Endpoints.v1.Encoding;

namespace One.INc.Web.API.Endpoints
{
    public static class EndpointsRegistrationExtention
    {
        private const double VersionOne = 1.0;
        public static void RegisterV1Endpoints(this WebApplication app) 
        {
            app.RegisterAuthEndpoint(VersionOne);
            app.RegisterEncodingEndpoint(VersionOne);
        }
    }
}
