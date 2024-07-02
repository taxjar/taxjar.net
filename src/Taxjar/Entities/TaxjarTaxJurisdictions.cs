using System.Text.Json.Serialization;

namespace Taxjar
{
    public record TaxJurisdictions
    {
        ///<summary>
        ///Two-letter ISO country code for given location.
        ///</summary>
        [JsonPropertyName("country")]
        public string Country { get; set; } = string.Empty;

        ///<summary>
        ///Postal abbreviated state name for given location.
        ///</summary>
        [JsonPropertyName("state")]
        public string State { get; set; } = string.Empty;

        ///<summary>
        ///County name for given location.
        ///</summary>
        [JsonPropertyName("county")]
        public string County { get; set; } = string.Empty;

        ///<summary>
        ///City name for given location.
        ///</summary>
        [JsonPropertyName("city")]
        public string City { get; set; } = string.Empty;

    }
}
