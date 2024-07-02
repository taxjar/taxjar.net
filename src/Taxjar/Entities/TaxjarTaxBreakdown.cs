using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Taxjar
{
    public record TaxBreakdown : Breakdown
    {
        
        ///<summary>
        ///Two-letter ISO country code for given location.
        ///</summary>
        [JsonPropertyName("state_tax_rate")]
        public decimal StateTaxRate { get; set; }

        ///<summary>
        ///Amount of sales tax to collect for the state.
        ///</summary>
        [JsonPropertyName("state_tax_collectable")]
        public decimal StateTaxCollectable { get; set; }

        ///<summary>
        ///Amount of the order to be taxed at the county tax rate.
        ///</summary>
        [JsonPropertyName("county_tax_collectable")]
        public decimal CountyTaxCollectable { get; set; }

        ///<summary>
        ///Amount of sales tax to collect for the city.
        ///</summary>
        [JsonPropertyName("city_tax_collectable")]
        public decimal CityTaxCollectable { get; set; }

        ///<summary>
        ///Amount of the order to be taxed at the special district tax rate.
        ///</summary>
        [JsonPropertyName("special_district_taxable_amount")]
        public decimal SpecialDistrictTaxableAmount { get; set; }

        ///<summary>
        ///Special district sales tax rate for given location
        ///</summary>
        [JsonPropertyName("special_tax_rate")]
        public decimal SpecialDistrictTaxRate { get; set; }

        ///<summary>
        ///Amount of sales tax to collect for the special district.
        ///</summary>
        [JsonPropertyName("special_district_tax_collectable")]
        public decimal SpecialDistrictTaxCollectable { get; set; }

        ///<summary>
        ///Breakdown of shipping rates if applicable.
        ///</summary>
        [JsonPropertyName("shipping")]
        public TaxBreakdownShipping? Shipping { get; set; }

        ///<summary>
        ///Breakdown of rates by line item if applicable.
        ///</summary>
        [JsonPropertyName("line_items")]
        public List<TaxBreakdownLineItem>? LineItems { get; set; }
    }
}
