namespace MaterialFader.Messages
{
    internal interface IBlinkSpeedMessage : IMessage
    {
        int Speed { get; }
    }
}