using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MaterialFader
{
    public class StateManager : IStateManager
    {
        private readonly FaderPort _fp;
        private readonly IDictionary<string, IStateMessageHandler> _handlers;

        public StateManager(FaderPort fp, IEnumerable<IStateMessageHandler> handlers)
        {
            _fp = fp;
            _handlers = handlers.ToDictionary(h => h.State, StringComparer.OrdinalIgnoreCase);
        }

        public async ValueTask ChangeState(string state)
        {
            if (!_handlers.TryGetValue(state, out var newState))
            {
                return;
            }

            var oldState = State;

            if (oldState != null)
            {
                _fp.OnButtonChange -= oldState.OnButtonEvent;
                _fp.OnSliderChange -= oldState.OnSliderEvent;
                await oldState.LeaveState().ConfigureAwait(false);
            }

            State = newState;
            _fp.OnButtonChange += newState.OnButtonEvent;
            _fp.OnSliderChange += newState.OnSliderEvent;
            await newState.EnterState().ConfigureAwait(false);
        }

        public IStateMessageHandler State { get; private set; }
    }
}
