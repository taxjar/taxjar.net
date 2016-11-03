using Newtonsoft.Json;

namespace Taxjar
{
	public class Shipping
	{
		[JsonProperty("taxable_amount")]
		public decimal TaxableAmount { get; set; }

		[JsonProperty("tax_collectable")]
		public decimal TaxCollectable { get; set; }

		[JsonProperty("combined_tax_rate")]
		public decimal CombinedTaxRate { get; set; }

		[JsonProperty("state_taxable_amount")]
		public decimal StateTaxableAmount { get; set; }

		[JsonProperty("state_sales_tax_rate")]
		public decimal StateSalesTaxRate { get; set; }

		[JsonProperty("state_amount")]
		public decimal StateAmount { get; set; }

		[JsonProperty("county_taxable_amount")]
		public decimal CountyTaxableAmount { get; set; }

		[JsonProperty("county_tax_rate")]
		public decimal CountyTaxRate { get; set; }

		[JsonProperty("county_amount")]
		public decimal CountyAmount { get; set; }

		[JsonProperty("city_taxable_amount")]
		public decimal CityTaxableAmount { get; set; }

		[JsonProperty("city_tax_rate")]
		public decimal CityTaxRate { get; set; }

		[JsonProperty("city_amount")]
		public decimal CityAmount { get; set; }

		[JsonProperty("special_taxable_amount")]
		public decimal SpecialDistrictTaxableAmount { get; set; }

		[JsonProperty("special_tax_rate")]
		public decimal SpecialDistrictTaxRate { get; set; }

		[JsonProperty("special_district_amount")]
		public decimal SpecialDistrictAmount { get; set; }

		// International
		[JsonProperty("country_taxable_amount")]
		public decimal CountryTaxableAmount { get; set; }

		[JsonProperty("country_tax_rate")]
		public decimal CountryTaxRate { get; set; }

		[JsonProperty("country_tax_collectable")]
		public decimal CountryTaxCollectable { get; set; }

		// Canada
		[JsonProperty("gst_taxable_amount")]
		public decimal GSTTaxableAmount { get; set; }

		[JsonProperty("gst_tax_rate")]
		public decimal GSTTaxRate { get; set; }

		[JsonProperty("gst")]
		public decimal GST { get; set; }

		[JsonProperty("pst_taxable_amount")]
		public decimal PSTTaxableAmount { get; set; }

		[JsonProperty("pst_tax_rate")]
		public decimal PSTTaxRate { get; set; }

		[JsonProperty("pst")]
		public decimal PST { get; set; }

		[JsonProperty("qst_taxable_amount")]
		public decimal QSTTaxableAmount { get; set; }

		[JsonProperty("qst_tax_rate")]
		public decimal QSTTaxRate { get; set; }

		[JsonProperty("qst")]
		public decimal QST { get; set; }
	}
}
