using System;
using System.Collections.Generic;
using System.Linq;

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
            var parts = msg.Split(':', StringSplitOptions.None);
            if (string.IsNullOrEmpty(parts[0]))
            {
                return null;
            }

            var command = parts[0];
            var arguments = parts[1..];

            foreach (var parser in _parsers.Where(mp => CommandMatches(mp, command) && ArgsMatch(mp, arguments)))
            {
                var message = parser.Parse(command, arguments);
                if (message != null)
                {
                    return message;
                }
            }

            return null;
        }

        private static bool CommandMatches(IMessageParser mp, string cmd)
            => mp.Command == null || string.Equals(mp.Command, cmd, StringComparison.OrdinalIgnoreCase);

        private static bool ArgsMatch(IMessageParser mp, string[] args) =>
            mp.Arguments.IsWithinRange(args.Length);
    }
}
