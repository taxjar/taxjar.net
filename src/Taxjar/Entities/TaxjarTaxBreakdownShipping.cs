using System.Text.Json.Serialization;

namespace Taxjar
{
    public record TaxBreakdownShipping : Breakdown
    {
        ///<summary>
        ///Amount of the shipping to be taxed at the state tax rate.
        ///</summary>
        [JsonPropertyName("state_sales_tax_rate")]
        public decimal StateSalesTaxRate { get; set; }

        ///<summary>
        ///Amount of the shipping to be taxed by state jurisdiction.
        ///</summary>
        [JsonPropertyName("state_amount")]
        public decimal StateAmount { get; set; }

        ///<summary>
        ///Amount of the shipping to be taxed by county jurisdiction.
        ///</summary>
        [JsonPropertyName("county_amount")]
        public decimal CountyAmount { get; set; }

        ///<summary>
        ///Amount of the shipping to be taxed by city jurisdiction.
        ///</summary>
        [JsonPropertyName("city_amount")]
        public decimal CityAmount { get; set; }

        ///<summary>
        ///Amount of the shipping to be taxed by special district jurisdiction.
        ///</summary>
        [JsonPropertyName("special_taxable_amount")]
        public decimal SpecialDistrictTaxableAmount { get; set; }

        ///<summary>
        ///Amount of the shipping to be taxed at the special district tax rate.
        ///</summary>
        [JsonPropertyName("special_tax_rate")]
        public decimal SpecialDistrictTaxRate { get; set; }

        [JsonPropertyName("special_district_amount")]
        public decimal SpecialDistrictAmount { get; set; }
    }
}
