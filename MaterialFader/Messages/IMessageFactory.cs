namespace MaterialFader.Messages
{
    public interface IMessageFactory
    {
        IMessage Parse(string msg);
    }
}
