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
}
