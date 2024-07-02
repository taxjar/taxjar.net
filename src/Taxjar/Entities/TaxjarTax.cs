using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Taxjar
{
     ///<summary>
     ///Sales tax for a given order. If available, returns a breakdown of rates by jurisdiction at the order, shipping, and line item level.
     ///</summary>
     public record TaxResponse
     {
          ///<summary>
          ///
          ///</summary>
          [JsonPropertyName("tax")]
          public TaxResponseAttributes? Tax { get; set; }
     }

     public record TaxResponseAttributes
     {
          ///<summary>
          ///Total amount of the order.
          ///</summary>
          [JsonPropertyName("order_total_amount")]
          public decimal OrderTotalAmount { get; set; }

          ///<summary>
          ///Total amount of shipping for the order.
          ///</summary>
          [JsonPropertyName("shipping")]
          public decimal Shipping { get; set; }

          ///<summary>
          ///Amount of the order to be taxed.
          ///</summary>
          [JsonPropertyName("taxable_amount")]
          public decimal TaxableAmount { get; set; } = 0;

          ///<summary>
          ///
          ///</summary>
          [JsonPropertyName("amount_to_collect")]
          public decimal AmountToCollect { get; set; } = 0;

          ///<summary>
          ///Overall sales tax rate of the order (amount_to_collect ÷ taxable_amount).
          ///</summary>
          [JsonPropertyName("rate")]
          public decimal Rate { get; set; } = 0;

          ///<summary>
          ///Whether or not you have nexus for the order based on an address on file, nexus_addresses parameter, or from_ parameters.
          ///</summary>
          [JsonPropertyName("has_nexus")]
          public bool HasNexus { get; set; }

          ///<summary>
          ///Freight taxability for the order.
          ///</summary>
          [JsonPropertyName("freight_taxable")]
          public bool FreightTaxable { get; set; }

          ///<summary>
          ///Origin-based or destination-based sales tax collection.
          ///</summary>
          [JsonPropertyName("tax_source")]
          public string TaxSource { get; set; } = string.Empty;

          ///<summary>
          ///
          ///</summary>
          [JsonPropertyName("exemption_type")]
          public string? ExemptionType { get; set; }
          ///<summary>
          ///Jurisdiction names for the order.
          ///</summary>
          [JsonPropertyName("jurisdictions")]
          public TaxJurisdictions? Jurisdictions { get; set; }

          ///<summary>
          ///Breakdown of rates by jurisdiction for the order, shipping, and individual line items. If has_nexus is false or no line items are provided, no breakdown is returned in the response.
          ///</summary>
          [JsonPropertyName("breakdown")]
          public TaxBreakdown? Breakdown { get; set; }
     }

     public record Tax
     {
          ///<summary>
          ///
          ///</summary>
          [JsonPropertyName("from_country")]
          public string? FromCountry { get; set; }

          ///<summary>
          ///
          ///</summary>
          [JsonPropertyName("from_zip")]
          public string? FromZip { get; set; }

          ///<summary>
          ///
          ///</summary>
          [JsonPropertyName("from_state")]
          public string? FromState { get; set; }

          ///<summary>
          ///
          ///</summary>
          [JsonPropertyName("from_city")]
          public string? FromCity { get; set; }

          ///<summary>
          ///
          ///</summary>
          [JsonPropertyName("from_street")]
          public string? FromStreet { get; set; }

          ///<summary>
          ///
          ///</summary>
          [JsonPropertyName("to_country")]
          public string ToCountry { get; set; } = string.Empty;

          ///<summary>
          ///
          ///</summary>
          [JsonPropertyName("to_zip")]
          public string ToZip { get; set; } = string.Empty;

          ///<summary>
          ///
          ///</summary>
          [JsonPropertyName("to_state")]
          public string ToState { get; set; } = string.Empty;

          ///<summary>
          ///
          ///</summary>
          [JsonPropertyName("to_city")]
          public string? ToCity { get; set; }

          ///<summary>
          ///
          ///</summary>
          [JsonPropertyName("to_street")]
          public string? ToStreet { get; set; }

          ///<summary>
          ///
          ///</summary>
          [JsonPropertyName("amount")]
          public decimal? Amount { get; set; }

          ///<summary>
          ///
          ///</summary>
          [JsonPropertyName("shipping")]
          public decimal? Shipping { get; set; }

          ///<summary>
          ///
          ///</summary>
          [JsonPropertyName("customer_id")]
          public string? CustomerId { get; set; }

          ///<summary>
          ///
          ///</summary>
          [JsonPropertyName("exemption_type")]
          public string? ExemptionType { get; set; }
          ///<summary>
          ///
          ///</summary>
          [JsonPropertyName("nexus_addresses")]
          public List<NexusAddress>? NexusAddresses { get; set; }

          ///<summary>
          ///
          ///</summary>
          [JsonPropertyName("line_items")]
          public List<TaxLineItem>? LineItems { get; set; }
     }

     public record TaxLineItem
     {
          ///<summary>
          ///
          ///</summary>
          [JsonPropertyName("id")]
          public string Id { get; set; } = string.Empty;

          ///<summary>
          ///
          ///</summary>
          [JsonPropertyName("quantity")]
          public int Quantity { get; set; }

          ///<summary>
          ///
          ///</summary>
          [JsonPropertyName("product_tax_code")]
          public string? ProductTaxCode { get; set; }

          ///<summary>
          ///
          ///</summary>
          [JsonPropertyName("unit_price")]
          public decimal? UnitPrice { get; set; }

          ///<summary>
          ///
          ///</summary>
          [JsonPropertyName("discount")]
          public decimal? Discount { get; set; }
     }
}
