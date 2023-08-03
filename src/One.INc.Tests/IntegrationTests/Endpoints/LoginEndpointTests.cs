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
    public class LoginEndpointTests : IntegrationServerBase
    {
        private const string EndpointPath = "api/v1/login";
        public LoginEndpointTests(IntegrationHostFactory integrationHostFactory) : base(integrationHostFactory, EndpointPath)
        {
        }

        [Theory]
        [InlineData("","")]
        [InlineData("", "abc")]
        [InlineData("abc", "")]
        public async Task Test_HandleAsyncLoginRequestHandler_When_InputInvalid_Result400(string pass, string user)
        {
            // Arrange
            var mockService = new Mock<IAuthService>();
            var encodingRequest = new AuthLoginRequest { Password = pass, Username = user };

            // Act 
            var response = await this.PostAsync(mockService.Object, encodingRequest);

            // Assert 
            HttpResponseMessageCheck(HttpStatusCode.BadRequest, response);
        }

        [Fact]
        public async Task Test_HandleAsyncLoginRequestHandler_When_ServiceException_Result500()
        {
            // Arrange 
            var mockService = new Mock<IAuthService>();
            mockService.Setup(x => x.LoginAsync(It.IsAny<AuthLoginRequest>())).ThrowsAsync(new Exception());
            
            var encodingRequest = new AuthLoginRequest { Password = "password", Username = "username" };

            // Act 
            var response = await this.PostAsync(mockService.Object, encodingRequest);

            // Assert 
            HttpResponseMessageCheck(HttpStatusCode.InternalServerError, response);
        }

        public async Task Test_HandleAsyncLoginRequestHandler_WhenAuthSuccess_Result200()
        {
            // Arrange 
            var mockService = new Mock<IAuthService>();
            mockService.Setup(x => x.LoginAsync(It.IsAny<AuthLoginRequest>())).ReturnsAsync(new AuthLoginResponse(true));

            var encodingRequest = new AuthLoginRequest { Password = "password", Username = "username" };

            // Act 
            var response = await this.PostAsync(mockService.Object, encodingRequest);

            // Assert 
            HttpResponseMessageCheck(HttpStatusCode.OK, response);
        }

        [Fact]
        public async Task Test_HandleAsyncLoginRequestHandler_WhenAuthUnSuccess_Result401()
        {
            // Arrange 
            var mockService = new Mock<IAuthService>();
            mockService.Setup(x => x.LoginAsync(It.IsAny<AuthLoginRequest>())).ReturnsAsync(new AuthLoginResponse(false));

            var encodingRequest = new AuthLoginRequest { Password = "password", Username = "username" };

            // Act 
            var response = await this.PostAsync(mockService.Object, encodingRequest);

            // Assert 
            HttpResponseMessageCheck(HttpStatusCode.Unauthorized, response);
        }
    }
}
