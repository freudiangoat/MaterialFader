using MaterialFader.Messages;
using System.Threading.Tasks;

namespace MaterialFader
{
    public interface IStateMessageHandler
    {
        string State { get; }

        Task EnterState();

        ValueTask HandleIncoming(IMessage msg);

        Task LeaveState();

        void OnButtonEvent(object sender, FaderPortButtonEventArgs evt);

        void OnSliderEvent(object sender, int pos);
    }
}