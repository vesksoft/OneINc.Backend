using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OneINc.Web.Common.Models.Requests;
using OneINc.Web.Common.Validators;
using OneINc.Web.Core.Interfaces;
using OneINc.Web.Core.Services;
using System.Xml.Linq;

namespace One.INc.Web.API.Endpoints.v1.Auth
{
    public static class AuthEndpointExtention
    {
        private const string Name = "Login";

        private static string EndpointUrl = "api/v{version:apiVersion}/login";

        public static void RegisterAuthEndpoint(this WebApplication app, double version)
        {
            var apiV1 = app.NewVersionedApi()
           .MapGroup(EndpointUrl)
           .HasApiVersion(version);

            apiV1
                .MapPost("/", HandleAsyncLoginRequestHandler()).AllowAnonymous()
                .WithName(Name)
                .AddEndpointFilter<BaseValidationFilter<AuthLoginRequest>>();
        }

        private static Func<AuthLoginRequest, IAuthService, Task<IResult>> HandleAsyncLoginRequestHandler()
        {
            return [AllowAnonymous] async ([FromBody] AuthLoginRequest request, IAuthService service) =>
            {
                var retVal = await service.LoginAsync(request);

                if (retVal != null && retVal.Success)
                {
                    return Results.Ok(retVal);
                }
                else 
                {
                    return Results.Unauthorized();
                }
            };
        }
    }
}
