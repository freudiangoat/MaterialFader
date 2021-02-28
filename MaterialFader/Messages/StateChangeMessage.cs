using System;

namespace MaterialFader.Messages
{
    public class StateChangeMessageParser : IMessageParser
    {
        public string Command => "State";

        public Type MessageType => typeof(StateChangeMessage);

        private class StateChangeMessage : IStateChangeMessage
        {
            public StateChangeMessage(string state)
            {
                NewState = state;
            }

            public string Type => "state";

            public string NewState { get; }
        }
    }
}
