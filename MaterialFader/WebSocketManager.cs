using MaterialFader.Messages;
using SuperSocket;
using SuperSocket.Channel;
using SuperSocket.WebSocket.Server;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MaterialFader
{
    public class WebSocketManager : ISessionHandler, IWebSocketSessionManager
    {
        private static readonly JsonSerializerOptions _jsonOpts;

        static WebSocketManager()
        {
            _jsonOpts = new JsonSerializerOptions();
            _jsonOpts.Converters.Add(new JsonStringEnumConverter(new LowerCaseNamingPolicy()));
        }

        private readonly ISet<IAppSession> _sessions = new HashSet<IAppSession>();
        private IStateManager _stateManager;

        public void RegisterStateManager(IStateManager stateManager)
            => _stateManager = stateManager;

        public ValueTask Broadcast<T>(T msg)
        {
            return Broadcast(JsonSerializer.Serialize(msg, _jsonOpts));
        }

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

        private class LowerCaseNamingPolicy : JsonNamingPolicy
        {
            public override string ConvertName(string name) =>
                name.ToLowerInvariant();
        }
    }
}
