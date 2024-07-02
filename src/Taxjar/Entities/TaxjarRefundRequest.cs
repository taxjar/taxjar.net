using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;


namespace Taxjar;

public record TaxjarRefundRequest
{
    /// <summary>
    /// Unique identifier of the given refund transaction.
    /// </summary>
    ///<remarks> 
    ///Required. The 'transaction_id' should only include alphanumeric characters, underscores, and dashes.
    ///<remarks>
    [JsonPropertyName("transaction_id")]
    public string TransactionId { get; set; } = string.Empty;

    /// <summary>
    /// Unique identifier of the corresponding order transaction for the refund.
    /// </summary>
    [JsonPropertyName("transaction_reference_id")]
    public string TransactionReferenceId { get; set; } = string.Empty;

    /// <summary>
    /// The date/time the transaction was originally recorded
    /// </summary>
    [JsonPropertyName("transaction_date")]
    public DateTime TransactionDate { get; set; }

    /// <summary>
    /// Source of where the transaction was originally recorded. Defaults to “api”. 
    /// </summary>
    [JsonPropertyName("provider")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Provider { get; set; }

    /// <summary>
    /// Type of exemption for the order: wholesale, government, marketplace, other, or non_exempt.
    /// </summary>
    [JsonPropertyName("exemption_type")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? ExemptionType { get; set; }

    /// <summary>
    /// Two-letter ISO country code of the country where the order shipped to.
    /// </summary>
    [JsonPropertyName("from_country")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? FromCountry { get; set; }

    /// <summary>
    /// Postal code where the order shipped to (5-Digit ZIP or ZIP+4). 
    /// </summary>
    [JsonPropertyName("from_zip")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? FromZip { get; set; }

    /// <summary>
    ///Two-letter ISO state code where the order shipped from. 
    /// </summary>
    [JsonPropertyName("from_state")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? FromState { get; set; }

    /// <summary>
    ///City where the order shipped from. 
    /// </summary>
    [JsonPropertyName("from_city")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? FromCity { get; set; }

    /// <summary>
    ///Street address where the order shipped from. 
    /// </summary>
    [JsonPropertyName("from_street")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? FromStreet { get; set; } = string.Empty;

    /// <summary>
    ///Two-letter ISO country code of the country where the order shipped to. 
    ///</summary>
    ///<remarks>
    ///required
    ///</remarks>
    [JsonPropertyName("to_country")]
    public string ToCountry { get; set; } = string.Empty;

    /// <summary>
    ///Postal code where the order shipped to (5-Digit ZIP or ZIP+4). 
    /// </summary>
    ///<remarks>
    ///required 
    ///</remarks>
    [JsonPropertyName("to_zip")]
    public string ToZip { get; set; } = string.Empty;

    /// <summary>
    ///Two-letter ISO state code where the order shipped to. 
    /// </summary>
    ///<remarks>
    ///required 
    ///</remarks>
    [JsonPropertyName("to_state")]
    public string ToState { get; set; } = string.Empty;

    /// <summary>
    ///City where the order shipped to.
    /// </summary>
    [JsonPropertyName("to_city")]
    public string ToCity { get; set; } = string.Empty;

    /// <summary>
    ///Street address where the order shipped to. 
    /// </summary>
    [JsonPropertyName("to_street")]
    public string ToStreet { get; set; } = string.Empty;

    ///<summary>
    ///Total amount of the refunded order with shipping, excluding sales tax in dollars.
    ///</summary>
    ///<remarks>
    ///required
    ///</remarks>
    [JsonPropertyName("amount")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public decimal Amount { get; set; }

    /// <summary>
    ///Total amount of shipping for the refunded order in dollars. 
    /// </summary>
    ///<remarks>
    ///required
    ///</remarks>
    [JsonPropertyName("shipping")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public decimal Shipping { get; set; }

    ///<summary>
    ///Total amount of sales tax collected for the refunded order in dollars. 
    ///</summary>
    ///<remarks>
    ///required
    ///</remarks>
    [JsonPropertyName("sales_tax")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public decimal SalesTax { get; set; }

    /// <summary>
    ///Unique identifier of the given customer for exemptions. 
    /// </summary>
    [JsonPropertyName("customer_id")]
    public string CustomerId { get; set; } = string.Empty;

    [JsonPropertyName("line_items")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public List<LineItem>? LineItems { get; set; }
}