using System.Text.Json.Serialization;

namespace Taxjar
{
    public record ValidationResponse
    {
        [JsonPropertyName("validation")]
        public ValidationResponseAttributes? Validation { get; set; }
    }

    public record ValidationResponseAttributes
    {
        [JsonPropertyName("valid")]
        public bool Valid { get; set; }

        [JsonPropertyName("exists")]
        public bool Exists { get; set; }

        [JsonPropertyName("vies_available")]
        public bool ViesAvailable { get; set; }

        [JsonPropertyName("vies_response")]
        public ViesResponse? ViesResponse { get; set; }
    }

    public record ViesResponse
    {
        [JsonPropertyName("country_code")]
        public string CountryCode { get; set; } = string.Empty;

        [JsonPropertyName("vat_number")]
        public string VatNumber { get; set; } = string.Empty;

        [JsonPropertyName("request_date")]
        public string RequestDate { get; set; } = string.Empty;

        [JsonPropertyName("valid")]
        public bool Valid { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("address")]
        public string Address { get; set; } = string.Empty;
    }

    public record Validation
    {
        [JsonPropertyName("vat")]
        public string Vat { get; set; } = string.Empty;
    }
}
