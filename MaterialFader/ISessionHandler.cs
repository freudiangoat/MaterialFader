using SuperSocket;
using SuperSocket.Channel;
using System.Threading.Tasks;

namespace MaterialFader
{
    public interface ISessionHandler
    {
        void RegisterStateManager(IStateManager stateManager);
        ValueTask OnConnected(IAppSession session);
        ValueTask OnDisconnected(IAppSession session, CloseEventArgs closeEvent);
    }
}