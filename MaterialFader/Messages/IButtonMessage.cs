namespace MaterialFader.Messages
{
    public interface IButtonMessage : IMessage
    {
        FaderPortButton Button { get; }
        FaderPortLightState Light { get; }
    }
}