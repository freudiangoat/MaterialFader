using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

namespace MaterialFader.Messages
{
    public class PingParser : IMessageParser
    {
        public string Command => "ping";

        public Type MessageType => typeof(PingMessage);

        private class PingMessage : IPingMessage
        {
            public PingMessage()
            {
            }

            [JsonPropertyName("type")]
            public string Type => "ping";
        }
    }
}
