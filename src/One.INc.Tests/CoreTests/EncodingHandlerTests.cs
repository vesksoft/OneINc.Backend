using Moq;
using OneINc.Web.Common.Models.Requests;
using OneINc.Web.Core.Queue;
using OneINc.Web.Core.Services;

namespace One.INc.Tests.CoreTests
{
    public class EncodingHandlerTests
    {
        [Fact]
        public async Task Test_InvokeEncodingAsync_WhenServiceThrowsException_ResultException() 
        {
            // Arrange 
            var encodingRequest = new EncodingRequest();
            var mockService = new Mock<IBackgroundTaskQueue>();
            mockService.Setup(x => x.QueueItemForWork(It.IsAny<EncodingRequest>())).Throws(new Exception());
            var handler = new EncodingHandler(mockService.Object);

            // Act 
            var sut = Record.ExceptionAsync(async () => await handler.InvokeEncodingAsync(encodingRequest));

            // Arrange
            Assert.NotNull(sut);
        }

        [Fact]
        public async Task Test_InvokeEncodingAsync_WhenServiceAcceptsREquest_ResultTrue()
        {
            // Arrange 
            var encodingRequest = new EncodingRequest();
            var mockService = new Mock<IBackgroundTaskQueue>();
            mockService.Setup(x => x.QueueItemForWork(It.IsAny<EncodingRequest>()));
            var handler = new EncodingHandler(mockService.Object);

            // Act 
            var sut = await handler.InvokeEncodingAsync(encodingRequest);

            // Arrange
            Assert.NotNull(sut);
            Assert.True(sut.Status);
        }

        [Fact]
        public async Task Test_CancelEncodingAsync_WhenServiceThrowsException_ResultException()
        {
            // Arrange 
            var sessionId = Guid.NewGuid();
            var mockService = new Mock<IBackgroundTaskQueue>();
            mockService.Setup(x => x.ClearQueueBySessionId(It.IsAny<Guid>())).Throws(new Exception());
            var handler = new EncodingHandler(mockService.Object);

            // Act 
            var sut = Record.ExceptionAsync(async () => { await handler.CancelEncodingAsync(sessionId); });

            // Arrange
            Assert.NotNull(sut);
        }

        [Fact]
        public async Task Test_CancelEncodingAsync_WhenServiceAcceptsREquest_ResultTrue()
        {
            // Arrange 
            var sessionId = Guid.NewGuid();
            var mockService = new Mock<IBackgroundTaskQueue>();
            mockService.Setup(x => x.ClearQueueBySessionId(It.IsAny<Guid>()));
            var handler = new EncodingHandler(mockService.Object);

            // Act 
            var sut = await handler.CancelEncodingAsync(sessionId);

            // Arrange
            Assert.True(sut);
        }
    }
}
