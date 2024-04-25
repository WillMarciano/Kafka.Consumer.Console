using System.Text.Json.Serialization;

namespace Kafka.Consumer.Console
{
    public class TemplateResponse
    {
        [JsonPropertyName("de")]
        public required string De { get; set; }

        [JsonPropertyName("para")]
        public string Para { get; set; }

        [JsonPropertyName("assunto")]
        public string Assunto { get; set; }
    }
}
