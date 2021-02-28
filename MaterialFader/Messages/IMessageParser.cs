using System;

namespace MaterialFader.Messages
{
    public interface IMessageParser
    {
        string Command { get; }

        Type MessageType { get; }
    }
}
