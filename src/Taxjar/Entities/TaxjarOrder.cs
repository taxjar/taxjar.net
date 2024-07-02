using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Taxjar
{
    public record OrdersResponse
    {
        ///<summary>
        ///
        ///</summary>
        [JsonPropertyName("orders")]
        public List<string>? Orders { get; set; }
    }

    public record OrderResponse
    {
        ///<summary>
        ///
        ///</summary>
        [JsonPropertyName("order")]
        public OrderResponseAttributes? Order { get; set; }
    }

    public record OrderResponseAttributes
    {
        ///<summary>
        /// Unique identifier of the given order transaction.
        ///</summary>
        [JsonPropertyName("transaction_id")]
        public string TransactionId { get; set; } = string.Empty;

        ///<summary>
        /// Unique identifier of the user who created the order transaction.
        ///</summary>
        [JsonPropertyName("user_id")]
        public int UserId { get; set; }

        ///<summary>
        /// The date/time the transaction was originally recorded.
        ///</summary>
        [JsonPropertyName("transaction_date")]
        public DateTime? TransactionDate { get; set; }

        ///<summary>
        ///Source of where the transaction was originally recorded.
        ///</summary>
        [JsonPropertyName("provider")]
        public string Provider { get; set; } = string.Empty;

        ///<summary>
        ///Type of exemption for the order: wholesale, government, marketplace, other, non_exempt, or null
        ///</summary>
        ///<remarks>
        ///A `null` value may be returned if the transaction does not have an exemption type or if `marketplace` is passed and a marketplace facilitator law does not apply in `to_state` as of the `transaction_date`.
        ///</remarks>
        [JsonPropertyName("exemption_type")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? ExemptionType { get; set; }

        ///<summary>
        ///Two-letter ISO country code of the country where the order shipped from.
        ///</summary>
        [JsonPropertyName("from_country")]
        public string FromCountry { get; set; } = string.Empty;

        ///<summary>
        ///Postal code where the order shipped from (5-Digit ZIP or ZIP+4).
        ///</summary>
        [JsonPropertyName("from_zip")]
        public string FromZip { get; set; } = string.Empty;

        ///<summary>
        /// Two-letter ISO state code where the order shipped from.
        ///</summary>
        [JsonPropertyName("from_state")]
        public string FromState { get; set; } = string.Empty;

        ///<summary>
        /// City where the order shipped from.
        ///</summary>
        [JsonPropertyName("from_city")]
        public string FromCity { get; set; } = string.Empty;

        ///<summary>
        ///Street address where the order shipped from.
        ///</summary>
        [JsonPropertyName("from_street")]
        public string FromStreet { get; set; } = string.Empty;

        ///<summary>
        ///Two-letter ISO country code of the country where the order shipped to.
        ///</summary>
        [JsonPropertyName("to_country")]
        public string ToCountry { get; set; } = string.Empty;

        ///<summary>
        /// 	Postal code where the order shipped to (5-Digit ZIP or ZIP+4).
        ///</summary>
        [JsonPropertyName("to_zip")]
        public string ToZip { get; set; } = string.Empty;

        ///<summary>
        ///Two-letter ISO state code where the order shipped to.
        ///</summary>
        [JsonPropertyName("to_state")]
        public string ToState { get; set; } = string.Empty;

        ///<summary>
        ///
        ///</summary>
        [JsonPropertyName("to_city")]
        public string ToCity { get; set; } = string.Empty;

        ///<summary>
        ///City where the order shipped to.
        ///</summary>
        [JsonPropertyName("to_street")]
        public string ToStreet { get; set; } = string.Empty;

        ///<summary>
        ///Street address where the order shipped to.
        ///</summary>
        [JsonPropertyName("amount")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public decimal? Amount { get; set; }

        ///<summary>
        ///Total amount of shipping for the order.
        ///</summary>
        [JsonPropertyName("shipping")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public decimal? Shipping { get; set; }

        ///<summary>
        ///Total amount of sales tax collected for the order.
        ///</summary>
        [JsonPropertyName("sales_tax")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public decimal? SalesTax { get; set; }

        ///<summary>
        ///
        ///</summary>
        [JsonPropertyName("line_items")]
        public List<LineItem>? LineItems { get; set; }
    }

    public record OrderFilter : ITaxjarOrderFilter
    {
        /// <summary>
        /// The date the transactions were originally recorded.
        /// </summary>
        [JsonPropertyName("transaction_date")]
        [JsonConverter(typeof(TaxjarDateTimeFilterJsonConverter))]
        public DateTime? TransactionDate {get; set;}

        /// <summary>
        /// Start date of a range for which the transactions were originally recorded.
        /// </summary>
        [JsonPropertyName("from_transaction_date")]
        [JsonConverter(typeof(TaxjarDateTimeFilterJsonConverter))]
        public DateTime? FromTransactionDate { get; set; }

        /// <summary>
        /// End date of a range for which the transactions were originally recorded.
        /// </summary>
        [JsonPropertyName("to_transaction_date")]
        [JsonConverter(typeof(TaxjarDateTimeFilterJsonConverter))]
        public DateTime? ToTransactionDate { get; set; }

        /// <summary>
        /// Source of where the transactions were originally recorded. Defaults to “api”.
        /// </summary>
        [JsonPropertyName("provider")]
        public string? Provider { get; set; }
    }

}
