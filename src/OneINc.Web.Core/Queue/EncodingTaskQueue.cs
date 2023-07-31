using OneINc.Web.Common.Models.Requests;
using OneINc.Web.Core.Interfaces;
using System.Collections.Concurrent;

namespace OneINc.Web.Core.Queue
{
    /// <summary>
    /// Encoding queque mechanism to correct add tasks to the queue
    /// </summary>
    public class EncodingTaskQueue : IBackgroundTaskQueue
    {
        private readonly IEncodingService _encodingService;
        private readonly BlockingCollection<Func<CancellationToken, Task>> workItems = new BlockingCollection<Func<CancellationToken, Task>>();
        public EncodingTaskQueue(IEncodingService encodingService) 
        { 
            _encodingService = encodingService ?? throw new ArgumentNullException(nameof(encodingService));
        }

        /// <inheritdoc/>
        public IEnumerable<Func<CancellationToken, Task>> GetConsumingEnumerable(CancellationToken token)
        {
            return this.workItems.GetConsumingEnumerable(token);
        }

        /// <inheritdoc/>
        public int GetQueueSize()
        {
            return this.workItems.Count;
        }

        /// <inheritdoc/>
        public async void QueueItemForWork(EncodingRequest workItemParameters)
        {
            try
            {
                var success = this.workItems.TryAdd(token => this._encodingService.EncodeAsync(workItemParameters));
            }
            catch
            {
                /// logging
            }
        }

        /// <inheritdoc/>
        public void ClearQueueBySessionId(Guid sessionsId) 
        {
            _encodingService.ClearSession(sessionsId);
        }
    }
}
