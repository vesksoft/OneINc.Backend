namespace OneINc.Web.Common
{
    public static class CancellationTokenExtensions
    {
        public static async Task SafeDelay(this CancellationToken cancellationToken, int millisecondsDelay)
        {
            try
            {
                await Task.Delay(millisecondsDelay, cancellationToken);
            }
            catch (TaskCanceledException)
            {

            }
        }

        public static async Task SafeDelay(this CancellationToken cancellationToken, TimeSpan delayTime)
        {
            try
            {
                await Task.Delay(delayTime, cancellationToken);
            }
            catch (TaskCanceledException)
            {

            }
        }

        public static async Task UntilCancelledAsync(this CancellationToken cancellationToken)
        {
            await cancellationToken.SafeDelay(Timeout.Infinite);
        }


    }
}
