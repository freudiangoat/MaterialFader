using System.Threading.Tasks;
using MaterialFader.Messages;

namespace MaterialFader
{
    public interface IWebSocketSessionManager
    {
        ValueTask Broadcast(string msg);

        ValueTask Broadcast<T>(T msg);
    }
}