using System;

namespace MaterialFader.Messages
{
    public class StateChangeMessageParser : IMessageParser
    {
        public IMessage Parse(string msg)
        {
            var parts = msg.Split("=");
            if (parts.Length != 2 || !string.Equals(parts[0], "state", StringComparison.OrdinalIgnoreCase))
            {
                return null;
            }

            return new StateChangeMessage(parts[1]);
        }

        private class StateChangeMessage : IStateChangeMessage
        {
            public StateChangeMessage(string state)
            {
                NewState = state;
            }

            public string NewState { get; }
        }
    }
}
