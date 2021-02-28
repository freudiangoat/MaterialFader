namespace MaterialFader.Messages
{
    internal interface ISliderMessage : IMessage
    {
        int Position { get; }

        int Percentage { get; }
    }
}
