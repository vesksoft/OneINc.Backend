using Asp.Versioning;
using FluentValidation;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using One.INc.Web.API.Endpoints;

using One.INc.Web.API.Middlewares;
using OneINc.Web.Common.Auth.Basic;
using OneINc.Web.Common.Models;
using OneINc.Web.Common.Models.Requests;
using OneINc.Web.Common.Swagger;
using OneINc.Web.Common.Validators;
using OneINc.Web.Core;
using OneINc.Web.Core.Hub;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace One.INc.Web.API
{
    public static class AppBuildExtention
    {
        /// <summary>
        /// Extention method to register swagger version
        /// </summary>
        /// <param name="builder">WebApplicationBuilder param</param>
        public static void RegisterDependencies(this WebApplicationBuilder builder)
        {
            builder.RegisterBasicWithSwaggerAuth();    
            builder.RegisterConfigOptions();
            builder.RegisterSignalR();
            builder.Services.CoreRegistration();
            
            builder.Services.AddValidatorsFromAssemblyContaining<Program>(ServiceLifetime.Singleton);
            
            builder.Services
                .AddEndpointsApiExplorer()
                .AddApiVersioning(options =>
                {
                    options.ReportApiVersions = true;
                    options.ApiVersionReader = new UrlSegmentApiVersionReader();
                })
                .AddApiExplorer(options =>
                {
                    // note: this option is only necessary when versioning by url segment. the SubstitutionFormat
                    // can also be used to control the format of the API version in route templates
                    options.SubstituteApiVersionInUrl = true;
                })
                .EnableApiVersionBinding();

            builder.Services.AddOptions();
            builder.Services.AddScoped<IValidator<AuthLoginRequest>, AuthLoginRequestValidator>();
            builder.Services.AddScoped<IValidator<EncodingRequest>, EncodingRequestValidator>();
        }

        /// <summary>
        /// Extention method to init and run app
        /// </summary>
        /// <param name="builder"></param>
        public static void InitAndRun(this WebApplicationBuilder builder)
        {
             var app = builder.Build();
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(options =>
                {
                    var descriptions = app.DescribeApiVersions();

                    // build a swagger endpoint for each discovered API version
                    foreach (var description in descriptions)
                    {
                        var url = $"/swagger/{description.GroupName}/swagger.json";
                        var name = description.GroupName.ToUpperInvariant();
                        options.SwaggerEndpoint(url, name);
                    }
                });
            }

            var httpUrl = builder.Configuration.GetSection(FrontendAppOptions.Name).GetValue<string>("httpUrl");
            var httpsUrl = builder.Configuration.GetSection(FrontendAppOptions.Name).GetValue<string>("httpsUrl");

            app.UseCors(x => x
                .WithOrigins(httpUrl, httpsUrl)
                .AllowCredentials()
                .AllowAnyMethod()
                .AllowAnyHeader());

            app.MapHub<SignalrEncodingHub>("/signalrencodinghub");
            app.RegisterV1Endpoints();
            app.UseHttpsRedirection();
            app.UseExceptionHandling();
            app.UseAuthentication();
            app.UseAuthorization();
            app.Run();
        }

        private static void RegisterBasicWithSwaggerAuth(this WebApplicationBuilder builder)
        {
            //Basic Authentication
            builder.Services.AddAuthentication().AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>
                    (BasicAuthenticationDefaults.AuthenticationScheme, null);
            
            builder.Services.AddAuthorization();
           
            builder.Services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
            
            builder.Services.AddSwaggerGen(options =>
            {
                options.OperationFilter<SwaggerDefaultValues>();
                options.AddSecurityDefinition(BasicAuthenticationDefaults.AuthenticationScheme,
                    new OpenApiSecurityScheme()
                    {
                        Name = "Authorization",
                        Type = SecuritySchemeType.Http,
                        Scheme = BasicAuthenticationDefaults.AuthenticationScheme,
                        In = ParameterLocation.Header,
                        Description = "Basic authorization header"
                    });
                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = BasicAuthenticationDefaults.AuthenticationScheme
                            }
                        },
                        new string[] { "Basic" }
                    }
                });
            });
        }

        private static void RegisterConfigOptions(this WebApplicationBuilder builder)
        {
            var config = builder.Configuration;
            builder.Services.AddOptions<RandomPauseOptions>()
                .Bind(config.GetSection(RandomPauseOptions.Name))
                .ValidateDataAnnotations()
                .ValidateOnStart();

            builder.Services.AddOptions<BasicAuthOptions>()
                .Bind(config.GetSection(BasicAuthOptions.Name))
                .ValidateDataAnnotations()
                .ValidateOnStart();


            builder.Services.AddOptions<FrontendAppOptions>()
                .Bind(config.GetSection(FrontendAppOptions.Name))
                .ValidateDataAnnotations()
                .ValidateOnStart();
        }

        private static void RegisterSignalR(this WebApplicationBuilder builder)
        {
            builder.Services.AddCors();
            builder.Services.AddSignalR();
        }
    }
}
