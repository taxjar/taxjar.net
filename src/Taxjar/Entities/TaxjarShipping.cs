using Newtonsoft.Json;

namespace Taxjar
{
	public class Shipping
	{
		[JsonProperty("taxable_amount")]
		public decimal TaxableAmount { get; set; }

		[JsonProperty("tax_collectable")]
		public decimal TaxCollectable { get; set; }

		[JsonProperty("state_amount")]
		public decimal StateAmount { get; set; }

		[JsonProperty("state_sales_tax_rate")]
		public decimal StateSalesTaxRate { get; set; }

		[JsonProperty("county_amount")]
		public decimal CountyAmount { get; set; }

		[JsonProperty("county_tax_rate")]
		public decimal CountyTaxRate { get; set; }

		[JsonProperty("city_amount")]
		public decimal CityAmount { get; set; }

		[JsonProperty("city_tax_rate")]
		public decimal CityTaxRate { get; set; }

		[JsonProperty("special_district_amount")]
		public decimal SpecialDistrictAmount { get; set; }

		[JsonProperty("special_tax_rate")]
		public decimal SpecialTaxRate { get; set; }
	}
}
