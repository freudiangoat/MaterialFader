using System;

namespace MaterialFader.Messages
{
    public class BlinkSpeedMessageParser : IMessageParser
    {
        public IMessage Parse(string msg)
        {
            var parts = msg.Split("=");
            if (parts.Length != 2 || !string.Equals(parts[0], "blinkspeed", StringComparison.OrdinalIgnoreCase))
            {
                return null;
            }

            if (!int.TryParse(parts[1], out var speed))
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
