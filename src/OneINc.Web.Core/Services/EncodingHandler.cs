using OneINc.Web.Common.Models.Requests;
using OneINc.Web.Common.Models.Responses;
using OneINc.Web.Core.Interfaces;
using OneINc.Web.Core.Queue;

namespace OneINc.Web.Core.Services
{
    /// <summary>
    /// Encoding Handler
    /// </summary>
    public class EncodingHandler : IEncodingHandler
    {
        private readonly IBackgroundTaskQueue _backgroundTaskQueue;
        public EncodingHandler(IBackgroundTaskQueue backgroundTaskQueue)
        {
            _backgroundTaskQueue = backgroundTaskQueue 
                ?? throw new ArgumentNullException(nameof(backgroundTaskQueue));
        }

        /// <inheritdoc/>
        public async Task<EncodingResponse> InvokeEncodingAsync(EncodingRequest encodingRequest) 
        {
            _backgroundTaskQueue.QueueItemForWork(encodingRequest);
            return new EncodingResponse(true);
        }

        /// <inheritdoc/>
        public async Task<bool> CancelEncodingAsync(Guid sessionId)
        {
            _backgroundTaskQueue.ClearQueueBySessionId(sessionId);
            return true;
        }
    }
}
