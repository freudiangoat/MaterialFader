namespace MaterialFader.Messages
{
    public struct ArgumentRange
    {
        public ArgumentRange(int expected)
            : this(expected, expected)
        {
        }

        public ArgumentRange(int min, int max)
        {
            Minimum = min;
            Maximum = max;
        }

        public static ArgumentRange None { get; } = new ArgumentRange(0);

        public static ArgumentRange Single { get; } = new ArgumentRange(1);

        public static ArgumentRange MoreThan(int n) =>
            new ArgumentRange(n + 1, int.MaxValue);

        public int Minimum { get; }
        public int Maximum { get; }

        public bool IsWithinRange(int val)
            => val >= Minimum && val <= Maximum;
    }
}
