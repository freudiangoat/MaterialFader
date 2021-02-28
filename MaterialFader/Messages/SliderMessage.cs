using System;
using System.Collections.Generic;
using System.Text;

namespace MaterialFader.Messages
{
    public class SliderMessageParser : IMessageParser
    {
        public IMessage Parse(string msg)
        {
            var parts = msg.Split("=");
            if (parts.Length != 2 || !string.Equals(parts[0], "slider", StringComparison.InvariantCultureIgnoreCase))
            {
                return null;
            }

            return int.TryParse(parts[1], out var percentage)
                ? new SliderMessage(percentage)
                : null;
        }

        private class SliderMessage : ISliderMessage
        {
            public SliderMessage(int percentage)
            {
                Position = (1024 * percentage) / 100;
                Percentage = percentage;
            }

            public int Position { get; }

            public int Percentage { get; }
        }
    }
}
