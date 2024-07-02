using System.Text.Json.Serialization;

namespace Taxjar
{
    public record TaxjarError
    {
        [JsonPropertyName("error")]
        public string Error { get; set; } = string.Empty;

        [JsonPropertyName("detail")]
        public string? Detail { get; set; }
        
        [JsonPropertyName("status")]
        [JsonConverter(typeof(TaxjarPolymorphicNumberJsonConverter))]
        public string StatusCode { get; set; } = string.Empty;
    }
}
