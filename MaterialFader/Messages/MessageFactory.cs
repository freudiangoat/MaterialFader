using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace MaterialFader.Messages
{
    public class MessageFactory : IMessageFactory
    {
        private readonly IEnumerable<IMessageParser> _parsers;

        public MessageFactory(IEnumerable<IMessageParser> parsers)
        {
            _parsers = parsers;
        }

        public IMessage Parse(string msg)
        {
            var typedMsg = JsonSerializer.Deserialize<TypedMessage>(msg);

            if (typedMsg.Type == null)
            {
                return null;
            }

            var command = typedMsg.Type;

            foreach (var parser in _parsers.Where(mp => CommandMatches(mp, command)))
            {
                var message = JsonSerializer.Deserialize(msg, parser.MessageType, WebSocketManager.JsonOpts) as IMessage;
                if (message != null)
                {
                    return message;
                }
            }

            return null;
        }

        private static bool CommandMatches(IMessageParser mp, string cmd)
            => mp.Command == null || string.Equals(mp.Command, cmd, StringComparison.OrdinalIgnoreCase);

        private class TypedMessage
        {
            [JsonPropertyName("type")]
            public string Type { get; set; }
        }
    }
}
