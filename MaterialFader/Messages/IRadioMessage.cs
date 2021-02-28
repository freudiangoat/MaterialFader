using System.Collections.Generic;

namespace MaterialFader.Messages
{
    public interface IRadioMessage : IMessage
    {
        IReadOnlyCollection<FaderPortButton> Buttons { get; }
        string Name { get; }
    }
}