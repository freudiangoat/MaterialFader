using System.Text.Json.Serialization;

namespace MaterialFader.Messages
{
    public interface IMessage
    {
        [JsonPropertyName("type")]
        public string Type { get; }
    }
}