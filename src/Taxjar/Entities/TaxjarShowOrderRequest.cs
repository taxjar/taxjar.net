using System.Text.Json.Serialization;

namespace Taxjar
{
    public record TaxjarShowOrderRequest
    {
        [JsonPropertyName("transactionId")]
        public string TransactionId { get; set; } = string.Empty;

        [JsonPropertyName("provider")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Provider { get; set; }
    }
}
