using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Taxjar
{
    [JsonUnmappedMemberHandling(JsonUnmappedMemberHandling.Disallow)]
    public record TaxjarTaxCalculationRequest
    {

        ///<summary>
        ///Type of exemption for the order: wholesale, government, marketplace, other, or non_exempt.
        ///</summary>
        ///<remarks>
        ///An `exemption_type` of `wholesale`, `government`, or `other` will result in an order being determined fully exempt. A value of `marketplace` or `non_exempt will return calculated tax amounts in the response, regardless of any potential customer exemption (determined via `customer_id`).
        ///</remarks>
        [JsonPropertyName("exemption_type")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? ExemptionType { get; set; }

        ///<summary>
        ///Two-letter ISO country code of the country where the order shipped from. 
        ///</summary>
        [JsonPropertyName("from_country")]
        public string? FromCountry { get; set; }

        ///<summary>
        ///Postal code where the order shipped to (5-Digit ZIP or ZIP+4).
        ///</summary>
        [JsonPropertyName("from_zip")]
        public string? FromZip { get; set; }

        ///<summary>
        ///Two-letter ISO state code where the order shipped from.
        ///</summary>
        [JsonPropertyName("from_state")]
        public string? FromState { get; set; }

        ///<summary>
        ///City where the order shipped from.
        ///</summary>
        [JsonPropertyName("from_city")]
        public string? FromCity { get; set; }

        ///<summary>
        ///Two-letter ISO state code where the order shipped from.
        ///</summary>
        [JsonPropertyName("from_street")]
        public string? FromStreet { get; set; }

        ///<summary>
        /// Two-letter ISO country code of the country where the order shipped to.
        ///</summary>
        ///<remarks>
        /// required
        ///</remarks>
        [JsonPropertyName("to_country")]
        public string ToCountry { get; set; } = string.Empty;

        ///<summary>
        /// Postal code where the order shipped to (5-Digit ZIP or ZIP+4).
        ///</summary>
        ///<remarks>
        /// required
        ///</remarks>
        [JsonPropertyName("to_zip")]
        public string ToZip { get; set; } = string.Empty;

        ///<summary>
        /// Two-letter ISO state code where the order shipped to.
        ///</summary>
        ///<remarks>
        /// required
        ///</remarks>
        [JsonPropertyName("to_state")]
        public string ToState { get; set; } = string.Empty;

        ///<summary>
        ///City where the order shipped to.
        ///</summary>
        [JsonPropertyName("to_city")]
        public string? ToCity { get; set; }

        ///<summary>
        ///Street address where the order shipped to.
        ///</summary>
        ///<remarks>
        ///Street address provides more accurate calculations for the following states: AR, AZ, CA, CO, CT, DC, FL, GA, HI, IA, ID, IN, KS, KY, LA, MA, MD, ME, MI, MN, MO, MS, NC, ND, NE, NJ, NM, NV, NY, OH, OK, PA, RI, SC, SD, TN, TX, UT, VA, VT, WA, WI, WV, WY
        ///</remarks>
        [JsonPropertyName("to_street")]
        public string? ToStreet { get; set; }

        ///<summary>
        ///Total amount of the order, excluding shipping.
        ///</summary>
        ///<remarks>
        ///Conditional. Either `amount` or `line_items` parameters are required to perform tax calculations.
        ///</remarks>
        [JsonPropertyName("amount")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public decimal? Amount { get; set; }

        ///<summary>
        /// Total amount of shipping for the order.
        ///</summary>
        ///<remarks>
        /// required
        ///</remarks>
        [JsonPropertyName("shipping")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public decimal? Shipping { get; set; }

        ///<summary>
        ///Unique identifier of the given customer for exemptions.
        ///</summary>
        [JsonPropertyName("customer_id")]
        public string? CustomerId { get; set; }

        ///<summary>
        ///List of items, fees, and services associated with the order.
        ///</summary>
        [JsonPropertyName("line_items")]
        public List<TaxLineItem>? LineItems { get; set; }

        ///<summary>
        ///
        ///</summary>
        [JsonPropertyName("nexus_addresses")]
        public List<NexusAddress>? NexusAddresses { get; set; }

    }
}
