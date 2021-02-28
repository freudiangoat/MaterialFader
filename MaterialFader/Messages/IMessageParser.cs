namespace MaterialFader.Messages
{
    public interface IMessageParser
    {
        string Command { get; }

        ArgumentRange Arguments { get; }

        IMessage Parse(string command, string[] args);
    }
}
