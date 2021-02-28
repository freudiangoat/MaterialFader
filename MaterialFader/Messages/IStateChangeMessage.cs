namespace MaterialFader.Messages
{
    internal interface IStateChangeMessage : IMessage
    {
        string NewState { get; }
    }
}