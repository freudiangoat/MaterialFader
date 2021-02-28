using System.Collections.Generic;

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
            foreach (var parser in _parsers)
            {
                var message = parser.Parse(msg);
                if (message != null)
                {
                    return message;
                }
            }

            return null;
        }
    }
}
