using MaterialFader.Messages;
using SuperSocket.WebSocket;
using SuperSocket.WebSocket.Server;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MaterialFader.StateHandlers
{
    internal sealed class DisconnectedStateHandler : IStateMessageHandler
    {
        private readonly FaderPort _fp;
        public DisconnectedStateHandler(FaderPort fp)
        {
            _fp = fp;
        }

        public string State => "Disconnected";

        public Task EnterState()
        {
            _fp.ClearAllLights();

            Blink(FaderPortButton.Left);
            Blink(FaderPortButton.Bank);
            Blink(FaderPortButton.Right);
            Blink(FaderPortButton.Off);
            Blink(FaderPortButton.Undo);
            Blink(FaderPortButton.User);
            Blink(FaderPortButton.Punch);
            Blink(FaderPortButton.Shift);
            Blink(FaderPortButton.Mix);
            Blink(FaderPortButton.Read);

            return Task.FromResult(true);
        }

        public ValueTask HandleIncoming(IMessage msg)
        {
            return new ValueTask();
        }

        public Task LeaveState()
        {
            Off(FaderPortButton.Left);
            Off(FaderPortButton.Bank);
            Off(FaderPortButton.Right);
            Off(FaderPortButton.Off);
            Off(FaderPortButton.Undo);
            Off(FaderPortButton.User);
            Off(FaderPortButton.Punch);
            Off(FaderPortButton.Shift);
            Off(FaderPortButton.Mix);
            Off(FaderPortButton.Read);

            return Task.FromResult(true);
        }

        public void OnButtonEvent(object _, FaderPortButtonEventArgs __)
        {
        }

        public void OnSliderEvent(object _, int __)
        {
        }

        private void Blink(FaderPortButton btn) => _fp.SetLight(btn, FaderPortLightState.Blink);

        private void Off(FaderPortButton btn) => _fp.SetLight(btn, FaderPortLightState.Off);
    }
}
