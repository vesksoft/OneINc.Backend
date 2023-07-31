using Microsoft.Extensions.Hosting;
using OneINc.Web.Common;
using System.Threading.Channels;

namespace OneINc.Web.Core.Queue
{
    /// <summary>
    /// Host service to ensure queueing and dequeue for encoding sessions 
    /// </summary>
    public class EncodingHostService : BackgroundService
    {
        private readonly IBackgroundTaskQueue _encodingTaskQueue;
        private readonly List<Task> _workTasks;
        private readonly Channel<Func<CancellationToken, Task>> _channel;
        private Task MainDutyTask;

        public EncodingHostService(IBackgroundTaskQueue encodingTaskQueue)
        {
            _encodingTaskQueue = encodingTaskQueue ?? throw new ArgumentNullException(nameof(encodingTaskQueue));
            _workTasks = new List<Task>();
            _channel = Channel.CreateBounded<Func<CancellationToken, Task>>(10);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            this.MainDutyTask = Task.Run(async () =>
            {
                _workTasks.AddRange(Enumerable.Range(0, 25).Select(i => this.ProcessItem(stoppingToken)));

                //add reader completion task
                _workTasks.Add(_channel.Reader.Completion);

                //reading task from queue
                _workTasks.Add(this.ReadItemFromQueue(stoppingToken));

                await stoppingToken.UntilCancelledAsync();

                //complete channel writer
                _channel.Writer.Complete();

                //wait for completion of reader and other tasks
                await Task.WhenAll(_workTasks);
            });

            await stoppingToken.UntilCancelledAsync();

            await Task.WhenAll(this.MainDutyTask);

            this.MainDutyTask = null;
        }

        private async Task ReadItemFromQueue(CancellationToken cancellationToken)
        {
            foreach (var queueItem in _encodingTaskQueue.GetConsumingEnumerable(cancellationToken))
            {
                await _channel.Writer.WriteAsync(queueItem, cancellationToken);
            }
        }

        private async Task ProcessItem(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                //code extracted from extension method in .netcoreapp3.1
                while (await _channel.Reader.WaitToReadAsync(cancellationToken).ConfigureAwait(false))
                {
                    if (_channel.Reader.TryRead(out var workItem))
                    {
                        try
                        {
                            await Task.WhenAll(workItem(cancellationToken));
                        }
                        catch (Exception ex)
                        {
                            //await this.logger.ErrorAsync($"Error occurred executing {nameof(workItem)}", ex);
                        }
                    }
                }
            }
        }
    }
}
