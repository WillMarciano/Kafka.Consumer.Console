using System.Text.Json.Serialization;

namespace Kafka.Consumer.Console
{
    public class TemplateResponse
    {
        [JsonPropertyName("De")]
        public required string De { get; set; }

        [JsonPropertyName("Para")]
        public string Para { get; set; }

        [JsonPropertyName("Assunto")]
        public string Assunto { get; set; }
    }
}
