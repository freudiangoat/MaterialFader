using System;

namespace MaterialFader.Messages
{
    public class BlinkSpeedMessageParser : IMessageParser
    {
        public string Command => "BlinkSpeed";

        public Type MessageType => typeof(BlinkSpeedMessage);

        private class BlinkSpeedMessage : IBlinkSpeedMessage
        {
            public BlinkSpeedMessage(int speed)
            {
                Speed = speed;
            }

            public string Type => "blinkSpeed";

            public int Speed { get; }
        }
    }
}
