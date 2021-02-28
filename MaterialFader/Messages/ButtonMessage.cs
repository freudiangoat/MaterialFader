namespace MaterialFader.Messages
{
    public class ButtonMessageParser : IMessageParser
    {
        public string Command => null;

        public ArgumentRange Arguments => ArgumentRange.Single;

        public IMessage Parse(string command, string[] args)
        {
            var buttonName = command;
            var stateName = "";

            if (args.Length > 1)
            {
                return null;
            }
            else if (args.Length == 1)
            {
                stateName = args[0];
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
