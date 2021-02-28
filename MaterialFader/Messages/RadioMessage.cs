using System;
using System.Collections.Generic;
using System.Linq;

namespace MaterialFader.Messages
{
    public class RadioMessageParser : IMessageParser
    {
        public string Command => "Radio";

        public Type MessageType => typeof(RadioMessage);

        private class RadioMessage : IRadioMessage
        {
            public RadioMessage(string name, IEnumerable<FaderPortButton> buttons)
            {
                Name = name;
                Buttons = buttons.ToArray();
            }

            public string Type => "radio";

            public string Name { get; }

            public IReadOnlyCollection<FaderPortButton> Buttons { get; }
        }
    }
}
