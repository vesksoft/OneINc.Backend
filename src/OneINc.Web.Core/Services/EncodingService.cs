using OneINc.Web.Common.Models.Requests;
using OneINc.Web.Core.Hub;
using OneINc.Web.Core.Interfaces;
using System.Collections.Concurrent;
using System.Text;

namespace OneINc.Web.Core.Services
{
    public class EncodingService : IEncodingService
    {
        private readonly SignalrEncodingHub _signalrEncodingHub;
        private readonly IRandomPauseService _randomPauseService;

        private readonly ConcurrentDictionary<Guid, bool> _sessionDic;
        public EncodingService(IRandomPauseService randomPauseService, SignalrEncodingHub signalrEncodingHub)
        {
            _signalrEncodingHub = signalrEncodingHub
                ?? throw new ArgumentNullException(nameof(signalrEncodingHub));

            _randomPauseService = randomPauseService 
                ?? throw new ArgumentNullException(nameof(randomPauseService));

            _sessionDic = new ConcurrentDictionary<Guid, bool>();
        }

        /// <inheritdoc/>
        public async Task EncodeAsync(EncodingRequest request, bool delayExecution = true) 
        {
            var found = _sessionDic.TryGetValue(request.SessionId, out bool _);

            if (!found && !string.IsNullOrWhiteSpace(request.Content)) 
            {
                _sessionDic.TryAdd(request.SessionId, true);

                await EncodeAndNotifyUIViaSignalRAsync(request.Content, request.SessionId, delayExecution);
                
                _sessionDic.TryRemove(request.SessionId, out _);
            }
            else 
            {
                _sessionDic.TryRemove(request.SessionId, out _);
            }
        }

        private async Task EncodeAndNotifyUIViaSignalRAsync(string strValue, Guid sessionId, bool delayExecution) 
        {
            var encodedText = Convert.ToBase64String(Encoding.UTF8.GetBytes(strValue));

            for (int i = 0; i < encodedText.Length; i++)
            {
                if (delayExecution)
                {
                    await _randomPauseService.DelayByRandomTimeAsync();
                }

                var lastElement = i == encodedText.Length - 1;

                var temp = $"{encodedText[i]}:{sessionId}:{lastElement}";

                var found = _sessionDic.TryGetValue(sessionId, out _);

                if (found)
                {
                    await _signalrEncodingHub.Clients.Caller.DisplayMessage(temp);
                }
                else
                {
                    break;
                }
            }
        }

        /// <inheritdoc/>
        public void ClearSession(Guid sessionId) 
        {
            var found = _sessionDic.TryGetValue(sessionId, out bool _);
            
            if (found) 
            {
                _sessionDic.TryRemove(sessionId, out _);
            }
        }
    }
}
