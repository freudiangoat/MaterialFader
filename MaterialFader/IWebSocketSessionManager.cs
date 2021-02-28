using System.Threading.Tasks;

namespace MaterialFader
{
    public interface IWebSocketSessionManager
    {
        ValueTask Broadcast(string msg);
    }
}