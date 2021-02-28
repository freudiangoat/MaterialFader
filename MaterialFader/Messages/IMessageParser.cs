using System;
using System.Collections.Generic;
using System.Text;

namespace MaterialFader.Messages
{
    public interface IMessageParser
    {
        IMessage Parse(string msg);
    }
}
