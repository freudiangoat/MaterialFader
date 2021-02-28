namespace MaterialFader.Messages
{
    public class BlinkSpeedMessageParser : IMessageParser
    {
        public string Command => "BlinkSpeed";

        public ArgumentRange Arguments => ArgumentRange.Single;

        public IMessage Parse(string _, string[] args)
        {
            if (args.Length != 1)
            {
                return null;
            }

            if (!int.TryParse(args[0], out var speed))
            {
                return null;
            }

            return new BlinkSpeedMessage(speed);
        }

        private class BlinkSpeedMessage : IBlinkSpeedMessage
        {
            public BlinkSpeedMessage(int speed)
            {
                Speed = speed;
            }

            public int Speed { get; }
        }
    }
}
