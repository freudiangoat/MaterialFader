using System;
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
            }

            return _session.Broadcast("received");
        }

        public Task LeaveState()
        {
            return Task.FromResult(true);
        }

        public async void OnButtonEvent(object sender, FaderPortButtonEventArgs evt)
        {
            if (evt.State == FaderPortButtonState.Released)
            {
                return;
            }

            _fp.SetLight(evt.Button, FaderPortLightState.Toggle);
            await _session.Broadcast(evt.Button.ToString());
        }

        public async void OnSliderEvent(object sender, int pos)
        {
            await _session.Broadcast($"slider={pos}");
        }
    }
}
