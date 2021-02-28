using System;

namespace MaterialFader
{
    public class FaderPortButtonEventArgs
    {
        public FaderPortButtonEventArgs(FaderPortButton btn, FaderPortButtonState state)
        {
            Button = btn;
            State = state;
        }

        public FaderPortButton Button { get; }

        public FaderPortButtonState State { get; }
    }
}