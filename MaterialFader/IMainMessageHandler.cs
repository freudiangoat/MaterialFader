using SuperSocket.WebSocket;
using SuperSocket.WebSocket.Server;
using System.Threading.Tasks;

namespace MaterialFader
{
    public interface IMainMessageHandler
    {
        ValueTask HandleIncoming(WebSocketSession session, WebSocketPackage pkg);
    }
}