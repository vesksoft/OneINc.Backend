using Moq;
using OneINc.Web.Common.Models.Requests;
using OneINc.Web.Core.Interfaces;
using OneINc.Web.Core.Queue;
using Xunit;

namespace One.INc.Tests.CoreTests
{
    public class EncodingTaskQueueTests
    {
        [Fact]
        public void Test_GetQueueSize_RetunsPossitiveNumber() 
        {
            // Arrange 
            var mockService = new Mock<IEncodingService>();
            var encodingTaskQueue = new EncodingTaskQueue(mockService.Object);
            
            // Act 
            var sut = encodingTaskQueue.GetQueueSize();

            // Assert
            Assert.True(sut >= 0);
        }

        [Fact]
        public async Task Test_GetConsumingEnumerable_ResultNoExceptionEmtpyList() 
        {
            // Arrange
            var mockService = new Mock<IEncodingService>();
            var encodingTaskQueue = new EncodingTaskQueue(mockService.Object);

            // Act 
            var sut = encodingTaskQueue.GetConsumingEnumerable(default);

            // Assert
            Assert.NotNull(sut);
        }

        [Fact]
        public async Task Test_QueueItemForWork_ResultNoExceptionEt()
        {
            // Arrange 
            var mockService = new Mock<IEncodingService>();
            var encodingTaskQueue = new EncodingTaskQueue(mockService.Object);

            // Act 
            var sut = Record.Exception(() => encodingTaskQueue.QueueItemForWork(new EncodingRequest { Content = "", SessionId = Guid.NewGuid()}));

            // Assert
            Assert.Null(sut);
        }
        [Fact]
        public async Task Test_QueueItemForWorkServiceThrowsException_ResultNoException()
        {
            // Arrange
            var mockService = new Mock<IEncodingService>();
            mockService.Setup(x => x.EncodeAsync(It.IsAny<EncodingRequest>(), It.IsAny<bool>())).ThrowsAsync(new Exception());
            var encodingTaskQueue = new EncodingTaskQueue(mockService.Object);

            // Act 
            var sut = Record.Exception(() => encodingTaskQueue.QueueItemForWork(new EncodingRequest { Content = "", SessionId = Guid.NewGuid() }));

            // Assert
            Assert.Null(sut);
        }

        [Fact]
        public void Test_ClearQueueBySessionIdNoException_ResultNoException()
        {
            // Arrange
            var mockService = new Mock<IEncodingService>();
            mockService.Setup(x => x.ClearSession(It.IsAny<Guid>())); 
            var encodingTaskQueue = new EncodingTaskQueue(mockService.Object);

            // Act 
            var sut = Record.Exception(() => encodingTaskQueue.QueueItemForWork(new EncodingRequest { Content = "", SessionId = Guid.NewGuid() }));

            // Assert
            Assert.Null(sut);
        }

        [Fact]
        public void Test_ClearQueueBySessionThrowsException_ResulException()
        {
            // Arrange
            var mockService = new Mock<IEncodingService>();
            mockService.Setup(x => x.ClearSession(It.IsAny<Guid>())).Throws(new Exception());
            var encodingTaskQueue = new EncodingTaskQueue(mockService.Object);

            // Act 
            var sut = Record.Exception(() => encodingTaskQueue.ClearQueueBySessionId(Guid.NewGuid()));

            // Assert
            Assert.NotNull(sut);
        }
    }
}
