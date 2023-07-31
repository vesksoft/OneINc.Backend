using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OneINc.Web.Common.Models.Requests;
using OneINc.Web.Common.Validators;
using OneINc.Web.Core.Interfaces;

namespace One.INc.Web.API.Endpoints.v1.Encoding
{
    public static class EncodingEndpointExtention
    {
        private static string EndpointUrl = "api/v{version:apiVersion}/encoding";
        private const string NamePost = "EncodingInvoke";
        private const string NameDelete = "EncodingDispose";
        public static void RegisterEncodingEndpoint(this WebApplication app, double version)
        {
            var apiV1Invoke = app.NewVersionedApi()
            .MapGroup(EndpointUrl)
            .HasApiVersion(version);

            apiV1Invoke
                .MapPost("/", HandleSessionInvokeAsync()).RequireAuthorization()
                .WithName(NamePost)
                .AddEndpointFilter<BaseValidationFilter<EncodingRequest>>();

            var apiV1Dispose = app.NewVersionedApi()
                .MapGroup(EndpointUrl)
                .HasApiVersion(version);

            apiV1Dispose
                .MapDelete("/{id}", HandleSessionDisposeAsync()).RequireAuthorization()
                .WithName(NameDelete);
        }

        private static Func<EncodingRequest, IEncodingHandler, Task<IResult>> HandleSessionInvokeAsync()
        {
            return [Authorize] async ([FromBody] EncodingRequest request, IEncodingHandler service) =>
            {
                var retVal = await service.InvokeEncodingAsync(request);

                return Results.Ok(retVal);
            };
        }

        private static Func<Guid, IEncodingHandler, Task<IResult>> HandleSessionDisposeAsync()
        {
            return async (Guid id, IEncodingHandler handler) =>
            {
                var retVal = await handler.CancelEncodingAsync(id);

                return Results.Ok(retVal);
            };
        }
    }
}