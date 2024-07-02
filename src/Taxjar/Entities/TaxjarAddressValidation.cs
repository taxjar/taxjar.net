using System.Collections.Generic;
using System.Text.Json.Serialization;


namespace Taxjar
{
    public record AddressValidationResponse
    {
        [JsonPropertyName("addresses")]
        public List<Address>? Addresses { get; set; }
    }

    public record Address
    {
        /// <summary>
        /// Two-letter ISO country code of the customer’s address. At this time only US addresses can be validated.
        /// </summary>
        [JsonPropertyName("country")]
        public string? Country { get; set; } = "US";

        /// <summary>
        ///Postal code of the customer’s address (5-Digit ZIP or ZIP+4). 
        /// </summary>
        [JsonPropertyName("zip")]
        public string? Zip { get; set; }

        /// <summary>
        ///Two-letter ISO state code of the customer’s address. 
        /// </summary>
        [JsonPropertyName("state")]
        public string? State { get; set; }

        /// <summary>
        ///City of the customer’s address. 
        /// </summary>
        [JsonPropertyName("city")]
        public string? City { get; set; }

        /// <summary>
        ///Street address of the customer’s address or the entire address as “freeform” input. 
        /// </summary>
        [JsonPropertyName("street")]
        public string? Street { get; set; }
    }
}
