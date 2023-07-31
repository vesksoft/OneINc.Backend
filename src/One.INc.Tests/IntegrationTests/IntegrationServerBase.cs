using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System.Net;
using System.Text;

namespace One.INc.Tests.IntegrationTests
{
    public abstract class IntegrationServerBase : IClassFixture<IntegrationHostFactory>
    {
        private readonly string endpointPath;
        private readonly IntegrationHostFactory factory;

        protected IntegrationServerBase(IntegrationHostFactory integrationHostFactory, string endpointPath)
        {
            this.factory = integrationHostFactory
                           ?? throw new ArgumentNullException(nameof(integrationHostFactory));

            this.endpointPath = endpointPath;
        }

        protected static void HttpResponseMessageCheck(HttpStatusCode expectedStatusCode, HttpResponseMessage response)
        {
            Assert.NotNull(response);
            Assert.NotNull(response.Content);
            Assert.Equal(expectedStatusCode, response.StatusCode);
        }

        protected async Task<HttpResponseMessage> GetAsync<TTService>(TTService mockService, string endPointUrl = "", CustomHeader? customHeader = null)
            where TTService : class
        {
            var requestUrl = this.endpointPath +
                             (!string.IsNullOrWhiteSpace(endPointUrl) ? $"/{endPointUrl}" : string.Empty);

            return await this.GetClient(mockService, customHeader).GetAsync(requestUrl);
        }

        protected async Task<HttpResponseMessage> DeleteAsync<TTService>(TTService mockService, string endPointUrl = "", CustomHeader? customHeader = null)
                    where TTService : class
        {
            var requestUrl = this.endpointPath +
                             (!string.IsNullOrWhiteSpace(endPointUrl) ? $"/{endPointUrl}" : string.Empty);

            return await this.GetClient(mockService, customHeader).DeleteAsync(requestUrl);
        }

        protected async Task<HttpResponseMessage> PostAsync<TTService, TTInput>(
            TTService mockService,
            TTInput input,
            string endPointUrl = "",
            CustomHeader? customHeader = null)
            where TTService : class
            where TTInput : class
        {
            var requestUrl = this.endpointPath +
                             (!string.IsNullOrWhiteSpace(endPointUrl) ? $"/{endPointUrl}" : string.Empty);

            using var content = CreateHttpContent(input);
            return await this.GetClient(mockService, customHeader).PostAsync(requestUrl, content);
        }

        private static StringContent CreateHttpContent<T>(T obj) => new(
            JsonConvert.SerializeObject(obj),
            Encoding.UTF8,
            "application/json");

        private HttpClient GetClient<TService>(TService service, CustomHeader? customHeader = null)
            where TService : class
        {
            var httpClient = this.factory.WithWebHostBuilder(
                    builder => builder
                        .ConfigureAppConfiguration((_, config) => config.AddJsonFile("appsettings.Test.json"))
                        .ConfigureTestServices(services => services.AddTransient(_ => service)))
                .CreateClient();

            if (customHeader != null)
            {
                httpClient.DefaultRequestHeaders.Add(customHeader.HeaderName, customHeader.HeaderValue);
            }

            return httpClient;
        }
    }
}
