using Microsoft.AspNetCore.Authentication;
using Microsoft.OpenApi.Models;
using OneINc.Web.Api.Middlewares;
using OneINc.Web.Common.Auth.Basic;
namespace OneINc.Web.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication().AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>
                (BasicAuthenticationDefaults.AuthenticationScheme, null);

            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(options => 
            {
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

        public void Configure(WebApplication app, IWebHostEnvironment env)
        {
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseExceptionHandling();

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
