namespace MaterialFader.Messages
{
    public interface IMessageHandler
    {
        void HandleMessage(IMessage msg);
    }
}
