using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using Moq;
using OneINc.Web.Common.Models.Requests;
using OneINc.Web.Common.Models.Responses;
using OneINc.Web.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace One.INc.Tests.IntegrationTests.Endpoints
{
    [Trait("Category", "Integration")]
    public class EncodingEndpointTests : IntegrationServerBase
    {
        private const string EndpointPath = "api/v1/encoding";
        public EncodingEndpointTests(IntegrationHostFactory integrationHostFactory) : base(integrationHostFactory, EndpointPath)
        {
        }

        private CustomHeader SetBasicAutorizationHeader()
        {
            return new CustomHeader("Authorization", "Basic dXNlcm5hbWU6cGFzc3dvcmQ=");
        }

        [Fact]
        public async Task Test_HandleSessionInvokeApiAsync_When_RequestIsNotAutorized_Result401() 
        {
            // Arrange
            var mockService = new Mock<IEncodingHandler>();
            var encodingRequest = new EncodingRequest { Content = "", SessionId = Guid.NewGuid() };

            // Act 
            var response = await this.PostAsync(mockService.Object, encodingRequest);

            // Assert 
            HttpResponseMessageCheck(HttpStatusCode.Unauthorized, response);
        }

        [Fact]
        public async Task Test_HandleSessionDisposeApiAsync_When_AutorizationFailed_Result401() 
        {
            // Arrange
            var mockService = new Mock<IEncodingHandler>();

            var sessionId = Guid.NewGuid().ToString();

            // Act 
            var response = await this.DeleteAsync(mockService.Object, sessionId);

            // Assert 
            HttpResponseMessageCheck(HttpStatusCode.Unauthorized, response);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("  ")]
        public async Task Test_HandleSessionInvokeApiAsync_When_RequestIsInvalid_Result400(string content)
        {
            // Arrange
            var mockService = new Mock<IEncodingHandler>();

            var encodingRequest = new EncodingRequest { Content = content, SessionId = Guid.NewGuid() };

            // Act 
            var response = await this.PostAsync(mockService.Object, encodingRequest, null, SetBasicAutorizationHeader());

            // Assert 
            HttpResponseMessageCheck(HttpStatusCode.BadRequest, response);
        }

        [Fact]
        public async Task Test_HandleSessionInvokeApiAsync_When_ExceptionOccurs_Result500()
        {
            // Arrange
            var mockService = new Mock<IEncodingHandler>();

            mockService
                .Setup(x => x.InvokeEncodingAsync(It.IsAny<EncodingRequest>()))
                .ThrowsAsync(new Exception());

            var encodingRequest = new EncodingRequest { Content = "abc", SessionId = Guid.NewGuid() };

            // Act 
            var response = await this.PostAsync(mockService.Object, encodingRequest, null, SetBasicAutorizationHeader());

            // Assert 
            HttpResponseMessageCheck(HttpStatusCode.InternalServerError, response);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task Test_HandleSessionInvokeApiAsync_When_EvetythingIsOk_Result200(bool status)
        {
            // Arrange
            var mockService = new Mock<IEncodingHandler>();

            mockService
                .Setup(x => x.InvokeEncodingAsync(It.IsAny<EncodingRequest>()))
                .ReturnsAsync(new EncodingResponse(status));

            var encodingRequest = new EncodingRequest { Content = "abc", SessionId = Guid.NewGuid() };

            // Act 
            var response = await this.PostAsync(mockService.Object, encodingRequest, null, SetBasicAutorizationHeader());

            // Assert 
            HttpResponseMessageCheck(HttpStatusCode.OK, response);
        }

        [Fact]
        public async Task Test_HandleSessionDisposeApiAsync_When_ServiceThrowsEx_Result500()
        {
            // Arrange
            var sessionId = Guid.NewGuid().ToString();
            var mockService = new Mock<IEncodingHandler>();
            mockService
                .Setup(x => x.CancelEncodingAsync(It.IsAny<Guid>()))
                .ThrowsAsync(new Exception());

            // Act 
            var response = await this.DeleteAsync(mockService.Object, sessionId, SetBasicAutorizationHeader());

            // Assert 
            HttpResponseMessageCheck(HttpStatusCode.InternalServerError, response);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task Test_HandleSessionDisposeApiAsync_When_AllGood_Result200(bool status)
        {
            // Arrange
            var sessionId = Guid.NewGuid().ToString();
            var mockService = new Mock<IEncodingHandler>();
            mockService
                .Setup(x => x.CancelEncodingAsync(It.IsAny<Guid>()))
                .ReturnsAsync(status);

            // Act 
            var response = await this.DeleteAsync(mockService.Object, sessionId, SetBasicAutorizationHeader());

            // Assert 
            HttpResponseMessageCheck(HttpStatusCode.OK, response);
        }
    }
}
