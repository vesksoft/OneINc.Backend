using Microsoft.AspNetCore.SignalR;
using Moq;
using OneINc.Web.Common.Models.Requests;
using OneINc.Web.Core.Hub;
using OneINc.Web.Core.Interfaces;
using OneINc.Web.Core.Services;

namespace One.INc.Tests.CoreTests
{
    public class EncodingServiceTests
    {
        [Fact]
        public async Task Test_EncodeAsync_WhenRequestNull_ResultExceptionOccurs()
        {
            // Arrange
            var mockSignalrR = new Mock<SignalrEncodingHub>();
            var mockPauseService = new Mock<IRandomPauseService>();
            var encodingService = new EncodingService(mockPauseService.Object, mockSignalrR.Object);

            // Act 
            var sut = await Record.ExceptionAsync(async () => { await encodingService.EncodeAsync(null, false); });

            // Assert 
            Assert.NotNull(sut);
            Assert.True(sut.Message != null);
        }

        [Fact]
        public async Task Test_EncodeAsync_RequestNotNullPauseServiceThrowsException_ResultException()
        {
            // Arrange
            var mockSignalrR = new Mock<SignalrEncodingHub>();
            var mockPauseService = new Mock<IRandomPauseService>();
            mockPauseService.Setup(x => x.DelayByRandomTimeAsync()).ThrowsAsync(new Exception());    
            var encodingService = new EncodingService(mockPauseService.Object, mockSignalrR.Object);

            // Act 
            var sut = await Record.ExceptionAsync(async () => { 
                await encodingService.EncodeAsync(new EncodingRequest {SessionId = Guid.NewGuid(), Content = "abc" }, false); });

            // Assert 
            Assert.NotNull(sut);
            Assert.True(sut.Message != null);
        }

        [Fact]
        public async Task Test_EncodeAsync_RequestNotNullSignalRThrowsException_ResultException()
        {
            // Arrange
            var mockSignalrR = new Mock<SignalrEncodingHub>();
            var mockPauseService = new Mock<IRandomPauseService>();
            mockPauseService.Setup(x => x.DelayByRandomTimeAsync()).ReturnsAsync(0);

            var encodingService = new EncodingService(mockPauseService.Object, mockSignalrR.Object);

            // Act 
            var sut = await Record.ExceptionAsync(async () => {
                await encodingService.EncodeAsync(new EncodingRequest { SessionId = Guid.NewGuid(), Content = "abc" }, false);
            });

            // Assert 
            Assert.NotNull(sut);
            Assert.True(sut.Message != null);
        }

        [Fact]
        public void Test_ClearSessionAsync_ResultNoException()
        {
            // Arrange
            var mockSignalrR = new Mock<SignalrEncodingHub>();
            var mockPauseService = new Mock<IRandomPauseService>();
            var encodingService = new EncodingService(mockPauseService.Object, mockSignalrR.Object);
            var sessionId = Guid.NewGuid();

            // Act 
            var sut = Record.Exception(() => encodingService.ClearSession(sessionId));

            // Assert 
            Assert.Null(sut);
        }
    }
}
