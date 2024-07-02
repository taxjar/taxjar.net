using System.Text.Json.Serialization;

namespace Taxjar
{
    public record Breakdown
    {
        [JsonPropertyName("taxable_amount")]
        public decimal TaxableAmount { get; set; }

        [JsonPropertyName("tax_collectable")]
        public decimal TaxCollectable { get; set; }

        [JsonPropertyName("combined_tax_rate")]
        public decimal CombinedTaxRate { get; set; }

        [JsonPropertyName("state_taxable_amount")]
        public decimal StateTaxableAmount { get; set; }

        [JsonPropertyName("county_taxable_amount")]
        public decimal CountyTaxableAmount { get; set; }

        [JsonPropertyName("county_tax_rate")]
        public decimal CountyTaxRate { get; set; }

        [JsonPropertyName("city_taxable_amount")]
        public decimal CityTaxableAmount { get; set; }

        [JsonPropertyName("city_tax_rate")]
        public decimal CityTaxRate { get; set; }

        // International
        [JsonPropertyName("country_taxable_amount")]
        public decimal CountryTaxableAmount { get; set; }

        [JsonPropertyName("country_tax_rate")]
        public decimal CountryTaxRate { get; set; }

        [JsonPropertyName("country_tax_collectable")]
        public decimal CountryTaxCollectable { get; set; }

        // Canada
        [JsonPropertyName("gst_taxable_amount")]
        public decimal GSTTaxableAmount { get; set; }

        [JsonPropertyName("gst_tax_rate")]
        public decimal GSTTaxRate { get; set; }

        [JsonPropertyName("gst")]
        public decimal GST { get; set; }

        [JsonPropertyName("pst_taxable_amount")]
        public decimal PSTTaxableAmount { get; set; }

        [JsonPropertyName("pst_tax_rate")]
        public decimal PSTTaxRate { get; set; }

        [JsonPropertyName("pst")]
        public decimal PST { get; set; }

        [JsonPropertyName("qst_taxable_amount")]
        public decimal QSTTaxableAmount { get; set; }

        [JsonPropertyName("qst_tax_rate")]
        public decimal QSTTaxRate { get; set; }

        [JsonPropertyName("qst")]
        public decimal QST { get; set; }
    }
}
