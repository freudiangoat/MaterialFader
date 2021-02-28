using System;
using System.Collections.Generic;
using System.Text;

namespace MaterialFader.Messages
{
    public class SliderMessageParser : IMessageParser
    {
        public string Command => "Slider";

        public ArgumentRange Arguments => ArgumentRange.Single;

        public IMessage Parse(string _, string[] args)
            => int.TryParse(args[0], out var percentage)
                ? new SliderMessage(percentage)
                : null;

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
