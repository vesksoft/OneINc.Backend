using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Hosting;

namespace One.INc.Tests.IntegrationTests
{
    public sealed class IntegrationHostFactory : WebApplicationFactory<Program>
    {
        protected override IHost CreateHost(IHostBuilder builder)
        {
            builder.ConfigureServices(_ => { });
            return base.CreateHost(builder);
        }
    }
}
