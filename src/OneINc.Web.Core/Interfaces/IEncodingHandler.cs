using OneINc.Web.Common.Models.Requests;
using OneINc.Web.Common.Models.Responses;

namespace OneINc.Web.Core.Interfaces
{
    public interface IEncodingHandler
    {
        /// <summary>
        /// Method to invoke encoding in a background 
        /// </summary>
        /// <param name="encodingRequest"></param>
        /// <returns></returns>
        Task<EncodingResponse> InvokeEncodingAsync(EncodingRequest encodingRequest);

        //Stop encoding process by sessionId
        Task<bool> CancelEncodingAsync(Guid sessionId);
    }
}
