using System;

namespace MaterialFader.Messages
{
    public class SliderMessageParser : IMessageParser
    {
        public string Command => "Slider";

        public Type MessageType => typeof(SliderMessage);
    }

    public class SliderMessage : ISliderMessage
    {
        public SliderMessage(int percentage)
        {
            Position = (1024 * percentage) / 100;
            Percentage = percentage;
        }

        public string Type => "analog";

        public int Position { get; }

        public int Percentage { get; }
    }
}
