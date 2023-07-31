using Microsoft.Extensions.Options;
using Moq;
using OneINc.Web.Common.Models;
using OneINc.Web.Core.Services;
using System.Diagnostics;

namespace One.INc.Tests.CoreTests
{
    public class RandomPauseServiceTests
    {
        [Fact]
        public async Task CheckDelayIntervalIsCorrect() 
        {
            // Arrange
            var rangeMin = 1;
            var rangeMax = 5;
            var secMult = 1000;

            var mockSettings = new Mock<IOptions<RandomPauseOptions>>();
            mockSettings.Setup(x=>x.Value).Returns(new RandomPauseOptions { StartValue = rangeMin, EndValue = rangeMax});
            var service = new RandomPauseService(mockSettings.Object);
            Stopwatch delayTime = new Stopwatch();
            // Act 
            delayTime.Start();
            var sut = await service.DelayByRandomTimeAsync();
            delayTime.Stop();
            var spentTime = delayTime.Elapsed.Seconds;

            // Assert 
            Assert.True(rangeMin*secMult <= sut);
            Assert.True(rangeMax * secMult >= sut);
            Assert.True(spentTime >= sut/1000);
        }
    }
}
