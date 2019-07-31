using Newtonsoft.Json;

namespace Taxjar
{
    public class TaxBreakdownShipping : Breakdown
    {
        [JsonProperty("state_sales_tax_rate")]
        public decimal StateSalesTaxRate { get; set; }

        [JsonProperty("state_amount")]
        public decimal StateAmount { get; set; }

        [JsonProperty("county_amount")]
        public decimal CountyAmount { get; set; }

        [JsonProperty("city_amount")]
        public decimal CityAmount { get; set; }

        [JsonProperty("special_taxable_amount")]
        public decimal SpecialDistrictTaxableAmount { get; set; }

        [JsonProperty("special_tax_rate")]
        public decimal SpecialDistrictTaxRate { get; set; }

        [JsonProperty("special_district_amount")]
        public decimal SpecialDistrictAmount { get; set; }
    }
}
