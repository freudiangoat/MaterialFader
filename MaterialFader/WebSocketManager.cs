using SuperSocket;
using SuperSocket.Channel;
using SuperSocket.WebSocket.Server;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MaterialFader
{
    public class WebSocketManager : ISessionHandler, IWebSocketSessionManager
    {
        private readonly ISet<IAppSession> _sessions = new HashSet<IAppSession>();
        private IStateManager _stateManager;

        public void RegisterStateManager(IStateManager stateManager)
            => _stateManager = stateManager;

        public async ValueTask Broadcast(string msg)
        {
            foreach (var session in _sessions.OfType<WebSocketSession>())
            {
                await session.SendAsync(msg)
                    .ConfigureAwait(false);
            }
        }

        public async ValueTask OnConnected(IAppSession session)
        {
            if (_sessions.Count == 0)
            {
                await _stateManager.ChangeState("Connected");
            }

            _sessions.Add(session);
        }

        public async ValueTask OnDisconnected(IAppSession session, CloseEventArgs closeEvent)
        {
            _sessions.Remove(session);

            if (_sessions.Count == 0)
            {
                await _stateManager.ChangeState("Disconnected");
            }
        }
    }
}
