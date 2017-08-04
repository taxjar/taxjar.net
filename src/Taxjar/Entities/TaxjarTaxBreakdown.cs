using System.Collections.Generic;
using Newtonsoft.Json;

namespace Taxjar
{
	public class TaxBreakdown : Breakdown
	{
		[JsonProperty("state_tax_rate")]
		public decimal StateTaxRate { get; set; }

		[JsonProperty("state_tax_collectable")]
		public decimal StateTaxCollectable { get; set; }

		[JsonProperty("county_tax_collectable")]
		public decimal CountyTaxCollectable { get; set; }

		[JsonProperty("city_tax_collectable")]
		public decimal CityTaxCollectable { get; set; }

		[JsonProperty("special_district_taxable_amount")]
		public decimal SpecialDistrictTaxableAmount { get; set; }

		[JsonProperty("special_tax_rate")]
		public decimal SpecialDistrictTaxRate { get; set; }

		[JsonProperty("special_district_tax_collectable")]
		public decimal SpecialDistrictTaxCollectable { get; set; }

		[JsonProperty("shipping")]
		public TaxBreakdownShipping Shipping { get; set; }

		[JsonProperty("line_items")]
		public List<TaxBreakdownLineItem> LineItems { get; set; }
	}
}
