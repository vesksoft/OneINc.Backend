using OneINc.Web.Common.Models.Requests;

namespace OneINc.Web.Core.Queue
{
    public interface IBackgroundTaskQueue
    {
        /// <summary>
        /// Method to add encodingRequest to the queue
        /// </summary>
        /// <param name="workItemParameters"></param>
        void QueueItemForWork(EncodingRequest workItemParameters);

        /// <summary>
        /// Consume queue by the host service 
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        
        IEnumerable<Func<CancellationToken, Task>> GetConsumingEnumerable(CancellationToken token);
        /// <summary>
        /// queue size
        /// </summary>
        /// <returns></returns>
        int GetQueueSize();

        /// <summary>
        /// direct method to stop encoding process by sessionId
        /// </summary>
        /// <param name="sessionsId"></param>
        void ClearQueueBySessionId(Guid sessionsId);
    }
}
