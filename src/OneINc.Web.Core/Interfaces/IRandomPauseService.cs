namespace OneINc.Web.Core.Interfaces
{
    /// <inheritdoc/>
    public interface IRandomPauseService
    {

        /// <summary>
        /// Method to provide random pause by milliseconds. Returns millesocond pause
        /// </summary>
        Task<int> DelayByRandomTimeAsync();
    }
}
