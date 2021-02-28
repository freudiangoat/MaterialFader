using System.Threading.Tasks;

namespace MaterialFader
{
    public interface IStateManager
    {
        IStateMessageHandler State { get; }

        ValueTask ChangeState(string state);
    }
}