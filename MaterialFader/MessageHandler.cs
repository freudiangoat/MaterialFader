using MaterialFader.Messages;
using SuperSocket.WebSocket;
using SuperSocket.WebSocket.Server;
using System.Threading.Tasks;

namespace MaterialFader
{
    public sealed class MessageHandler : IMainMessageHandler
    {
        private readonly IStateManager _stateManager;
        private readonly IMessageFactory _messageFactory;
        private readonly FaderPort _fp;

        public MessageHandler(FaderPort fp, IStateManager stateManager, IMessageFactory messageFactory)
        {
            _stateManager = stateManager;
            _messageFactory = messageFactory;
            _fp = fp;
        }

        public ValueTask HandleIncoming(WebSocketSession session, WebSocketPackage pkg)
        {
            var msg = _messageFactory.Parse(pkg.Message);
            if (msg == null)
            {
                return new ValueTask();
            }

            if (msg is IStateChangeMessage stateChange)
            {
                return _stateManager.ChangeState(stateChange.NewState);
            }
            else if (msg is IBlinkSpeedMessage speedChange)
            {
                _fp.BlinkPeriodMs = speedChange.Speed > 0
                    ? speedChange.Speed
                    : 500;

                return new ValueTask();
            }

            return _stateManager.State.HandleIncoming(msg);
        }
    }
}
