using OneINc.Web.Common.Models.Requests;
using OneINc.Web.Common.Models.Responses;

namespace OneINc.Web.Core.Interfaces
{
    public interface IEncodingService
    {
        /// <summary>
        /// Method to encoding string to string64
        /// </summary>
        /// <param name="content">string to be encoded</param>
        /// <param name="delayExecution">optional parameter to delay or not delay execution of the method</param>
        /// <returns></returns>
        Task EncodeAsync(EncodingRequest request, bool delayExecution = true);

        /// <summary>
        /// Method to stop encoding process for specific sessionId
        /// </summary>
        /// <param name="sessionId"></param>
        void ClearSession(Guid sessionId);
    }
}
