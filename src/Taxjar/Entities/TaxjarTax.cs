using System.Collections.Generic;
using Newtonsoft.Json;

namespace Taxjar
{
	public class TaxRequest
	{
		[JsonProperty("tax")]
		public Tax Tax { get; set; }
	}

	public class Tax
	{
		[JsonProperty("order_total_amount")]
		public decimal OrderTotalAmount { get; set; }

		[JsonProperty("shipping")]
		public decimal Shipping { get; set; }

		[JsonProperty("taxable_amount")]
		public decimal TaxableAmount { get; set; }

		[JsonProperty("amount_to_collect")]
		public decimal AmountToCollect { get; set; }

		[JsonProperty("rate")]
		public decimal Rate { get; set; }

		[JsonProperty("has_nexus")]
		public bool HasNexus { get; set; }

		[JsonProperty("freight_taxable")]
		public bool FreightTaxable { get; set; }

		[JsonProperty("tax_source")]
		public string TaxSource { get; set; }

		[JsonProperty("breakdown")]
		public TaxBreakdown Breakdown { get; set; }
	}

	public class TaxBreakdown
	{
		[JsonProperty("state_taxable_amount")]
		public decimal StateTaxableAmount { get; set; }

		[JsonProperty("state_tax_collectable")]
		public decimal StateTaxCollectable { get; set; }

		[JsonProperty("county_taxable_amount")]
		public decimal CountyTaxableAmount { get; set; }

		[JsonProperty("county_tax_collectable")]
		public decimal CountyTaxCollectable { get; set; }

		[JsonProperty("city_taxable_amount")]
		public decimal CityTaxableAmount { get; set; }

		[JsonProperty("city_tax_collectable")]
		public decimal CityTaxCollectable { get; set; }

		[JsonProperty("special_district_taxable_amount")]
		public decimal SpecialDistrictTaxableAmount { get; set; }

		[JsonProperty("special_district_tax_collectable")]
		public decimal SpecialDistrictTaxCollectable { get; set; }

		[JsonProperty("shipping")]
		public Shipping Shipping { get; set; }

		[JsonProperty("line_items")]
		public List<TaxBreakdownLineItem> LineItems { get; set; }
	}

	public class TaxBreakdownLineItem
	{
		[JsonProperty("id")]
		public int Id { get; set; }

		[JsonProperty("tax_collectable")]
		public decimal TaxCollectable { get; set; }

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

		[JsonProperty("special_district_taxable_amount")]
		public decimal SpecialDistrictTaxableAmount { get; set; }

		[JsonProperty("special_tax_rate")]
		public decimal SpecialTaxRate { get; set; }

		[JsonProperty("special_district_amount")]
		public decimal SpecialDistrictAmount { get; set; }
	}
}
