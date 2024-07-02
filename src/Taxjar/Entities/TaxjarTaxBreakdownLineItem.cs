using System.Text.Json.Serialization;

namespace Taxjar
{
    public record TaxBreakdownLineItem : Breakdown
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("state_sales_tax_rate")]
        public decimal StateSalesTaxRate { get; set; }

        [JsonPropertyName("state_amount")]
        public decimal StateAmount { get; set; }

        [JsonPropertyName("county_amount")]
        public decimal CountyAmount { get; set; }

        [JsonPropertyName("city_amount")]
        public decimal CityAmount { get; set; }

        [JsonPropertyName("special_district_taxable_amount")]
        public decimal SpecialDistrictTaxableAmount { get; set; }

        [JsonPropertyName("special_tax_rate")]
        public decimal SpecialTaxRate { get; set; }

        [JsonPropertyName("special_district_amount")]
        public decimal SpecialDistrictAmount { get; set; }
    }
}
