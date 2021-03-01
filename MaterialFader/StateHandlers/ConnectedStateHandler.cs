using System;
using System.Linq;
using System.Threading.Tasks;

using MaterialFader.Messages;

namespace MaterialFader.StateHandlers
{
    public class ConnectedStateHandler : IStateMessageHandler
    {
        private readonly FaderPort _fp;
        private readonly IWebSocketSessionManager _session;

        public ConnectedStateHandler(FaderPort fp, IWebSocketSessionManager session)
        {
            _fp = fp;
            _session = session;
        }

        public string State => "Connected";

        public Task EnterState()
        {
            foreach (FaderPortButton btn in Enum.GetValues(typeof(FaderPortButton)))
            {
                _fp.SetLight(btn, FaderPortLightState.Off);
            }

            return Task.FromResult(true);
        }

        public ValueTask HandleIncoming(IMessage msg)
        {
            switch (msg)
            {
                case IButtonMessage btnMessage:
                    _fp.SetLight(btnMessage.Button, btnMessage.Light);
                    break;
                case ISliderMessage sliderMessage:
                    _fp.SetSlider(sliderMessage.Position);
                    break;
                case IRadioMessage radioMessage:
                    _fp.AddRadioGroup(radioMessage.Name, radioMessage.Buttons);
                    break;
                case ISetBindingsMessage setBindingsMessage:
                    foreach (var btn in Enum.GetValues<FaderPortButton>())
                    {
                        _fp.SetLight(btn, setBindingsMessage.BoundButtons.Contains(btn) ? FaderPortLightState.On : FaderPortLightState.Off);
                    }

                    break;
            }

            return ValueTask.CompletedTask;
        }

        public Task LeaveState()
        {
            return Task.FromResult(true);
        }

        public async void OnButtonEvent(object sender, FaderPortButtonEventArgs evt)
        {
            if (evt.State == FaderPortButtonState.Released)
            {
                _fp.SetLight(evt.Button, FaderPortLightState.Toggle);
                return;
            }

            _fp.SetLight(evt.Button, FaderPortLightState.Toggle);

            var msg = new ButtonMessage(evt.Button, evt.State == FaderPortButtonState.Pushed, FaderPortLightState.Off);
            await _session.Broadcast(msg);
        }

        public async void OnSliderEvent(object sender, int pos)
        {
            var msg = new SliderMessage(pos);
            await _session.Broadcast(msg);
        }
    }
}
