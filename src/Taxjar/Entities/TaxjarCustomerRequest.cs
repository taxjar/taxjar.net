using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Taxjar
{
    public record TaxjarCustomerRequest
    {

        ///<summary>
        ///Unique identifier of the given customer.
        ///</summary>
        ///<remarks>
        /// required
        //</remarks>
        [JsonPropertyName("customer_id")]
        public string CustomerId { get; set; } = string.Empty;

        ///<summary>
        ///Type of customer exemption: wholesale, government, other, or non_exempt.
        ///</summary>
        ///<remarks>
        /// required
        //</remarks>
        [JsonPropertyName("exemption_type")]
        public string ExemptionType { get; set; } = "non_exempt";

        ///<summary>
        ///Two-letter ISO country code where the customer is exempt.Two-letter ISO state code where the customer is exempt.
        ///</summary>
        ///<remarks>
        ///If no exempt regions are provided, the customer will be treated as exempt or non-exempt everywhere.
        //</remarks>
        [JsonPropertyName("exempt_regions")]
        public List<ExemptRegion>? ExemptRegions { get; set; } = null;

        ///<summary>
        ///Name of the customer.
        ///</summary>
        ///<remarks>
        /// required
        //</remarks>
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        ///<summary>
        ///Two-letter ISO country code of the customer’s primary address.
        ///</summary>
        [JsonPropertyName("country")]
        public string? Country { get; set; }

        ///<summary>
        /// Two-letter ISO state code of the customer’s primary address.
        ///</summary>
        [JsonPropertyName("state")]
        public string? State { get; set; }

        ///<summary>
        ///Postal code of the customer’s primary address.
        ///</summary>
        [JsonPropertyName("zip")]
        public string? Zip { get; set; }

        ///<summary>
        ///City of the customer’s primary address.
        ///</summary>
        [JsonPropertyName("city")]
        public string? City { get; set; }

        ///<summary>
        ///Street address of the customer’s primary address.
        ///</summary>
        [JsonPropertyName("street")]
        public string? Street { get; set; }
    }
}
