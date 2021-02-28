namespace MaterialFader.Messages
{
    public class StateChangeMessageParser : IMessageParser
    {
        public string Command => "State";

        public ArgumentRange Arguments => ArgumentRange.Single;

        public IMessage Parse(string _, string[] args)
            => new StateChangeMessage(args[0]);

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
