using Microsoft.Extensions.DependencyInjection;
using OneINc.Web.Core.Hub;
using OneINc.Web.Core.Interfaces;
using OneINc.Web.Core.Queue;
using OneINc.Web.Core.Services;

namespace OneINc.Web.Core
{
    public static class ServiceRegistrationExtention
    {
        public static void CoreRegistration(this IServiceCollection services) 
        {
            services.AddSingleton<IEncodingService, EncodingService>();
            services.AddSingleton<IRandomPauseService, RandomPauseService>();
            services.AddTransient<IAuthService, AuthService>();
            services.AddSingleton<IEncodingHandler, EncodingHandler>();
            services.AddSingleton<IBackgroundTaskQueue, EncodingTaskQueue>();
            services.AddHostedService<EncodingHostService>();
            services.AddSingleton<SignalrEncodingHub>();
        }
    }
}
