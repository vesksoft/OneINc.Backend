using Microsoft.Extensions.Options;
using OneINc.Web.Common.Models;
using OneINc.Web.Core.Interfaces;

namespace OneINc.Web.Core.Services
{
    /// <summary>
    /// Specific service to generate random pause 
    /// </summary>
    public class RandomPauseService : IRandomPauseService
    {
        private const int SecondMultiplier = 1000;
        private readonly RandomPauseOptions _randomPauseOptions;
        private readonly Random _randomTimeProvider;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="configuration">config settings to provided start and end of random generator</param>
        /// <exception cref="ArgumentNullException">Exception will appear if incorrect settings loaded</exception>
        public RandomPauseService(IOptions<RandomPauseOptions> configuration) 
        {
            _randomPauseOptions = configuration?.Value ?? throw new ArgumentNullException(nameof(configuration));
            _randomTimeProvider = new Random();
        }

        /// <inheritdoc/>
        public async Task<int> DelayByRandomTimeAsync()
        {
            var currentPause = _randomTimeProvider
                .Next(_randomPauseOptions.StartValue, _randomPauseOptions.EndValue) * SecondMultiplier;

            await Task.Delay(currentPause);

            return currentPause;
        }
    }
}
