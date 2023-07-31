using Microsoft.Extensions.Options;
using Moq;
using OneINc.Web.Common.Models;
using OneINc.Web.Common.Models.Requests;
using OneINc.Web.Core.Services;

namespace One.INc.Tests.CoreTests
{
    public class AuthServiceTests
    {
        [Fact]
        public async Task Test_LoginAsync_WhenRequestNull_ResultFalse()
        {
            // Arrange
            AuthLoginRequest request = default;
            var mockSettings = new Mock<IOptions<BasicAuthOptions>>();
            mockSettings.Setup(x => x.Value)
                .Returns(new BasicAuthOptions { NameValue = "name", PasswordValue ="pass"});
            var authService = new AuthService(mockSettings.Object);
            
            // Act 
            var sut = await authService.LoginAsync(request);

            // Assert
            Assert.True(sut != null);
            Assert.False(sut.Success);
        }

        [Fact]
        public async Task Test_LoginAsync_WhenRequestInValid_ResultFalse()
        {
            // Arrange
            AuthLoginRequest request = new AuthLoginRequest 
            {
                Password = "p",
                Username = "u",
            };
            var mockSettings = new Mock<IOptions<BasicAuthOptions>>();
            mockSettings.Setup(x => x.Value)
                .Returns(new BasicAuthOptions { NameValue = "name", PasswordValue = "pass" });
            var authService = new AuthService(mockSettings.Object);

            // Act 
            var sut = await authService.LoginAsync(request);

            // Assert
            Assert.True(sut != null);
            Assert.False(sut.Success);
        }

        [Fact]
        public async Task Test_LoginAsync_WhenRequestCorrect_ResulTrue()
        {
            // Arrange
            AuthLoginRequest request = new AuthLoginRequest 
            {
                Password = "pass",
                Username = "name"
            };
            var mockSettings = new Mock<IOptions<BasicAuthOptions>>();
            mockSettings.Setup(x => x.Value)
                .Returns(new BasicAuthOptions { NameValue = "name", PasswordValue = "pass" });
            var authService = new AuthService(mockSettings.Object);

            // Act 
            var sut = await authService.LoginAsync(request);

            // Assert
            Assert.True(sut != null);
            Assert.True(sut.Success);
        }
    }
}
