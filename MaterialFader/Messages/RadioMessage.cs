using System.Collections.Generic;
using System.Linq;

namespace MaterialFader.Messages
{
    public class RadioMessageParser : IMessageParser
    {
        public string Command => "Radio";

        public ArgumentRange Arguments { get; } = ArgumentRange.MoreThan(1);

        public IMessage Parse(string command, string[] args)
            => new RadioMessage(args[0], args.Skip(1).AsValidEnums<FaderPortButton>());

        private class RadioMessage : IRadioMessage
        {
            public RadioMessage(string name, IEnumerable<FaderPortButton> buttons)
            {
                Name = name;
                Buttons = buttons.ToArray();
            }

            public string Name { get; }
            public IReadOnlyCollection<FaderPortButton> Buttons { get; }
        }
    }
}
