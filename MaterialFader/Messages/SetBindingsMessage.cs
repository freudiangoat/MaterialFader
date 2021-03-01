using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

namespace MaterialFader.Messages
{
    public class SetBindingsParser : IMessageParser
    {
        public string Command => "set_bindings";

        public Type MessageType { get; } = typeof(SetBindingsMessage);
    }

    public class SetBindingsMessage : ISetBindingsMessage
    {
        public SetBindingsMessage()
        {
        }

        [JsonPropertyName("boundButtons")]
        public List<FaderPortButton> BoundButtons { get; set; }

        public string Type => "set_bindings";
    }
}