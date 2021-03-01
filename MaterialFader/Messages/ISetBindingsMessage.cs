using System.Collections.Generic;

namespace MaterialFader.Messages
{
    public interface ISetBindingsMessage : IMessage
    {
        List<FaderPortButton> BoundButtons { get; }
    }
}