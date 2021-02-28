using Melanchall.DryWetMidi.Common;
using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Devices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace MaterialFader
{
    public class FaderPort : IDisposable
    {
        private readonly IDictionary<FaderPortButton, FaderPortLightState> _lightState = new Dictionary<FaderPortButton, FaderPortLightState>();
        private readonly IDictionary<string, ISet<FaderPortButton>> _radioGroups = new Dictionary<string, ISet<FaderPortButton>>();
        private IInputDevice _inputDevice;
        private IOutputDevice _outputDevice;
        private bool _blinkState;
        private Thread _blinkerThread;
        private CancellationTokenSource _cts;

        private ControlChangeEvent _pendingSlider;
        private int _sliderPos;

        public FaderPort()
        {
            _inputDevice = InputDevice.GetByName("FaderPort");
            _outputDevice = OutputDevice.GetByName("FaderPort");

            if (_inputDevice == null)
            {
                throw new InvalidOperationException("Unable to connect to FaderPort input device.");
            }

            if (_outputDevice == null)
            {
                throw new InvalidOperationException("Unable to connect to FaderPort output device.");
            }

            _inputDevice.EventReceived += OnInputEvent;
            _inputDevice.StartEventsListening();
            _outputDevice.PrepareForEventsSending();

            var nativeEvent = new NoteOnEvent((SevenBitNumber)0, (SevenBitNumber)0x64)
            {
                Channel = (FourBitNumber)1
            };

            _outputDevice.SendEvent(nativeEvent);
            ClearAllLights();

            _cts = new CancellationTokenSource();
            _blinkerThread = new Thread(BlinkerMain);
            _blinkerThread.Start();
        }

        public int BlinkPeriodMs { get; set; } = 500;

        public int SliderPosition
        {
            get
            {
                return _sliderPos;
            }

            private set
            {
                _sliderPos = value;
                OnSliderChange?.Invoke(this, value * 100 / 0x4000);
            }
        }

        public void Dispose()
        {
            ClearAllLights();
            _cts.Cancel();
            _cts.Dispose();
            _inputDevice.StopEventsListening();
        }

        public void ClearAllLights()
        {
            foreach (FaderPortButton btn in Enum.GetValues(typeof(FaderPortButton)))
            {
                SetLight(btn, FaderPortLightState.Off);
            }
        }

        private void BlinkerMain()
        {
            while (!_cts.IsCancellationRequested)
            {
                KeyValuePair<FaderPortButton, FaderPortLightState>[] copy;
                lock(_lightState)
                {
                    copy = _lightState.ToArray();
                }

                foreach (var button in copy.Where(b => b.Value == FaderPortLightState.Blink))
                {
                    SetLight(button.Key, _blinkState);
                }

                _blinkState = !_blinkState;

                Thread.Sleep(BlinkPeriodMs);
            }
        }

        public void SetLight(FaderPortButton button, FaderPortLightState state)
        {
            lock(_lightState)
            {
                if (state == FaderPortLightState.Toggle)
                {
                    var oldState = _lightState[button];

                    // Everything except a current setting of Off results in a new state of Off
                    state = oldState == FaderPortLightState.Off
                        ? FaderPortLightState.On
                        : FaderPortLightState.Off;
                }

                _lightState[button] = state;
            }

            SetLight(button, state != FaderPortLightState.Off);
        }

        private void SetLight(FaderPortButton button, bool on)
        {
            if ((int)button > 0x7f)
            {
                return;
            }

            var radioGroup = GetRadioGroup(button);

            if (radioGroup != null)
            {
                if (!on)
                {
                    return;
                }

                foreach (var btn in radioGroup)
                {
                    _lightState[btn] = FaderPortLightState.Off;
                    _outputDevice.SendEvent(new NoteAftertouchEvent(Out((byte)btn), Out(false)));
                }
            }

            _outputDevice.SendEvent(new NoteAftertouchEvent(Out((byte)button), Out(on)));
        }

        public void SetSlider(int position)
        {
            position = Math.Max(0, Math.Min(1024, position));
            var sliderMoveHi = new ControlChangeEvent((SevenBitNumber)0, OutRaw(position >> 7));
            var sliderMoveLo = new ControlChangeEvent((SevenBitNumber)0x20, OutRaw(position));
            _outputDevice.SendEvent(sliderMoveHi);
            _outputDevice.SendEvent(sliderMoveLo);
        }

        public void AddRadioGroup(string name, IEnumerable<FaderPortButton> buttons)
        {
            if (!_radioGroups.TryGetValue(name, out var buttonSet))
            {
                buttonSet = new HashSet<FaderPortButton>();
                _radioGroups[name] = buttonSet;
            }

            foreach (var btn in buttons)
            {
                var oldSet = GetRadioGroup(btn);
                if (oldSet == buttonSet)
                {
                    continue;
                }

                oldSet?.Remove(btn);
                buttonSet.Add(btn);
            }
        }

        public void RemoveRadioGroup(string name)
            => _radioGroups.Remove(name);

        public event EventHandler<FaderPortButtonEventArgs> OnButtonChange;

        public event EventHandler<int> OnSliderChange;

        private ISet<FaderPortButton> GetRadioGroup(FaderPortButton btn)
            => _radioGroups.FirstOrDefault(g => g.Value.Contains(btn)).Value;

        private static SevenBitNumber OutRaw(int val) => (SevenBitNumber)(byte)(val & 0x7F);

        private static SevenBitNumber Out(byte val) => (SevenBitNumber)(byte)((val & 0xF8) | ((~val) & 0x07));

        private static SevenBitNumber Out(bool val) => (SevenBitNumber)(val ? 1 : 0);

        private void OnInputEvent(object sender, MidiEventReceivedEventArgs e)
        {
            switch (e.Event)
            {
                case NoteAftertouchEvent aftertouchEvent:
                    var buttonEvt = new ButtonEvent(aftertouchEvent);
                    OnButtonChange?.Invoke(this, new FaderPortButtonEventArgs(buttonEvt.Button, buttonEvt.State));
                    return;
                case ControlChangeEvent controlChangeEvent:
                    {
                        if (_pendingSlider == null)
                        {
                            _pendingSlider = controlChangeEvent;
                        }
                        else if (_pendingSlider.ControlNumber == 0x20)
                        {
                            SliderPosition = _pendingSlider.ControlValue + (controlChangeEvent.ControlValue << 7);
                            _pendingSlider = null;
                        }
                        else
                        {
                            _pendingSlider = controlChangeEvent;
                        }
                    }
                    break;
                case PitchBendEvent pitchBendEvent:
                    var up = pitchBendEvent.PitchValue == 0x80;
                    break;
            }
        }

        private class ButtonEvent
        {
            public ButtonEvent(NoteAftertouchEvent evt)
            {
                Button = ParseButton(evt.NoteNumber);
                State = ParseState(evt.AftertouchValue);
            }

            public FaderPortButton Button { get; }

            public FaderPortButtonState State { get; }

            public override string ToString()
            {
                return $"{Button} {State} ({Convert.ToString((int)Button, 2)})";
            }

            private static FaderPortButton ParseButton(int evt)
                => (FaderPortButton)evt;

            private static FaderPortButtonState ParseState(int evt)
                => (FaderPortButtonState)evt;
        }
    }
}
