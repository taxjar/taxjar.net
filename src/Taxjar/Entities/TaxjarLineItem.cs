using System.Text.Json.Serialization;

namespace Taxjar
{
    public record LineItem
    {
        ///<summary>
        ///Unique identifier of the given line item.
        ///</summary>
        ///<remarks>
        ///Either `amount` or `line_items` parameters are required to perform tax calculations.
        ///</remarks>
        [JsonPropertyName("id")]
        [JsonConverter(typeof(TaxjarPolymorphicNumberJsonConverter))]
        public string Id { get; set; } = string.Empty;

        ///<summary>
        ///Quantity for the item.
        ///</summary>
        [JsonPropertyName("quantity")]
        public int Quantity { get; set; }

        ///<summary>
        ///Product identifier for the item.
        ///</summary>
        [JsonPropertyName("product_identifier")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? ProductIdentifier { get; set; }

        ///<summary>
        ///Product identifier for the item.
        ///</summary>
        [JsonPropertyName("description")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Description { get; set; }

        ///<summary>
        ///Tax code of the given product category.
        ///</summary>
        [JsonPropertyName("product_tax_code")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? ProductTaxCode { get; set; }

        ///<summary>
        ///Unit price for the item.
        ///</summary>
        [JsonPropertyName("unit_price")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public decimal? UnitPrice { get; set; }

        ///<summary>
        ///Total discount (non-unit) for the item.
        ///</summary>
        [JsonPropertyName("discount")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public decimal? Discount { get; set; }

        ///<summary>
        ///Total amount of sales tax collected for the order in dollars.
        ///</summary>
        [JsonPropertyName("sales_tax")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public decimal? SalesTax { get; set; }
    }
}
