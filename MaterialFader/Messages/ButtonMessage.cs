namespace MaterialFader.Messages
{
    public class ButtonMessageParser : IMessageParser
    {
        public IMessage Parse(string msg)
        {
            var buttonName = msg;
            var stateName = "";

            var parts = msg.Split("=");
            if (parts.Length > 2)
            {
                return null;
            }
            else if (parts.Length == 2)
            {
                buttonName = parts[0];
                stateName = parts[1];
            }

            if (!stateName.AsEnum<FaderPortLightState>(out var state))
            {
                state = FaderPortLightState.On;
            }

            if (buttonName.AsEnum<FaderPortButton>(out var btn))
            {
                return new ButtonMessage(btn, state);
            }

            return null;
        }

        private class ButtonMessage : IButtonMessage
        {
            public ButtonMessage(FaderPortButton btn, FaderPortLightState state)
            {
                Button = btn;
                Light = state;
            }

            public FaderPortButton Button { get; }

            public FaderPortLightState Light { get; }
        }
    }
}
