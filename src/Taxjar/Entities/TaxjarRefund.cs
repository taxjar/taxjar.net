using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Taxjar
{
    public record RefundsResponse
    {
        [JsonPropertyName("refunds")]
        public List<string>? Refunds { get; set; }
    }

    public record RefundResponse
    {
        [JsonPropertyName("refund")]
        public RefundResponseAttributes? Refund { get; set; }
    }

    public record RefundResponseAttributes
    {
        /// <summary>
        /// Unique identifier of the given refund transaction.
        /// </summary>
        /// <remarks>
        /// The 'transaction_id' should only include alphanumeric characters, underscores, and dashes.
        ///</remarks>
        [JsonPropertyName("transaction_id")]
        public string TransactionId { get; set; } = string.Empty;

        /// <summary>
        /// Unique identifier of the corresponding order transaction for the refund.
        /// </summary>
        [JsonPropertyName("transaction_reference_id")]
        public string TransactionReferenceId { get; set; } = string.Empty;

        /// <summary>
        /// Unique identifier of the user who created the order transaction. 
        /// </summary>
        [JsonPropertyName("user_id")]
        public int UserId { get; set; }

        /// <summary>
        /// The date/time the transaction was originally recorded. 
        /// </summary>
        [JsonPropertyName("transaction_date")]
        public DateTime? TransactionDate { get; set; }

        /// <summary>
        /// Source of where the transaction was originally recorded. 
        /// </summary>
        [JsonPropertyName("provider")]
        public string Provider { get; set; } = string.Empty;

        /// <summary>
        ///Type of exemption for the order: wholesale, government, marketplace, other, non_exempt, or null.  
        /// </summary>
        [JsonPropertyName("exemption_type")]
        public string? ExemptionType { get; set; }

        [JsonPropertyName("from_country")]
        public string? FromCountry { get; set; }

        [JsonPropertyName("from_zip")]
        public string? FromZip { get; set; }

        [JsonPropertyName("from_state")]
        public string? FromState { get; set; }
        [JsonPropertyName("from_city")]
        public string? FromCity { get; set; }

        [JsonPropertyName("from_street")]
        public string? FromStreet { get; set; }

        [JsonPropertyName("to_country")]
        public string ToCountry { get; set; } = string.Empty;

        [JsonPropertyName("to_zip")]
        public string? ToZip { get; set; } = string.Empty;

        [JsonPropertyName("to_state")]
        public string ToState { get; set; } = string.Empty;

        [JsonPropertyName("to_city")]
        public string? ToCity { get; set; }

        [JsonPropertyName("to_street")]
        public string? ToStreet { get; set; }

        [JsonPropertyName("amount")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public decimal? Amount { get; set; }

        [JsonPropertyName("shipping")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public decimal? Shipping { get; set; }

        [JsonPropertyName("sales_tax")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public decimal? SalesTax { get; set; }

        [JsonPropertyName("line_items")]
        public List<LineItem>? LineItems { get; set; }
    }

    public record Refund
    {
        [JsonPropertyName("transaction_id")]
        public string TransactionId { get; set; } = string.Empty;

        [JsonPropertyName("transaction_reference_id")]
        public string TransactionReferenceId { get; set; } = string.Empty;

        [JsonPropertyName("transaction_date")]
        public DateTime TransactionDate { get; set; }

        [JsonPropertyName("provider")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string Provider { get; set; } = string.Empty;

        [JsonPropertyName("exemption_type")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string? ExemptionType { get; set; }
        [JsonPropertyName("from_country")]
        public string FromCountry { get; set; } = string.Empty;

        [JsonPropertyName("from_zip")]
        public string FromZip { get; set; } = string.Empty;

        [JsonPropertyName("from_state")]
        public string FromState { get; set; } = string.Empty;

        [JsonPropertyName("from_city")]
        public string FromCity { get; set; } = string.Empty;

        [JsonPropertyName("from_street")]
        public string FromStreet { get; set; } = string.Empty;

        [JsonPropertyName("to_country")]
        public string ToCountry { get; set; } = string.Empty;

        [JsonPropertyName("to_zip")]
        public string ToZip { get; set; } = string.Empty;

        [JsonPropertyName("to_state")]
        public string ToState { get; set; } = string.Empty;

        [JsonPropertyName("to_city")]
        public string ToCity { get; set; } = string.Empty;

        [JsonPropertyName("to_street")]
        public string ToStreet { get; set; } = string.Empty;

        [JsonPropertyName("amount")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public decimal Amount { get; set; }

        [JsonPropertyName("shipping")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public decimal Shipping { get; set; }

        [JsonPropertyName("sales_tax")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public decimal SalesTax { get; set; }

        [JsonPropertyName("customer_id")]
        public string CustomerId { get; set; } = string.Empty;

        [JsonPropertyName("line_items")]
        public List<LineItem>? LineItems { get; set; }
    }

    public record RefundFilter : ITaxjarOrderFilter
    {
        /// <summary>
        /// The date the transactions were originally recorded.
        /// </summary>
        [JsonPropertyName("transaction_date")]
        [JsonConverter(typeof(TaxjarDateTimeFilterJsonConverter))]
        public DateTime? TransactionDate { get; set; }

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
