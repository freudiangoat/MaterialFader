using System;
using System.Text.Json.Serialization;

namespace MaterialFader.Messages
{
    public class ButtonMessageParser : IMessageParser
    {
        public string Command => "button";

        public Type MessageType => typeof(ButtonMessage);
    }

    public class ButtonMessage : IButtonMessage
    {
        public ButtonMessage(FaderPortButton button, bool pressed, FaderPortLightState light)
        {
            Button = button;
            Pressed = pressed;
            Light = light;
        }

        [JsonPropertyName("type")]
        public string Type => "button";

        [JsonPropertyName("name")]
        public FaderPortButton Button { get; }

        [JsonPropertyName("pressed")]
        public bool Pressed { get; }

        [JsonPropertyName("light")]
        public FaderPortLightState Light { get; }
    }
}
