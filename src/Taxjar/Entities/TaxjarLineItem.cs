using Newtonsoft.Json;

namespace Taxjar
{
	public class LineItem
	{
		[JsonProperty("id")]
		public string Id { get; set; }

		[JsonProperty("quantity")]
		public int Quantity { get; set; }

		[JsonProperty("product_identifier")]
		public string ProductIdentifier { get; set; }

		[JsonProperty("description")]
		public string Description { get; set; }

		[JsonProperty("product_tax_code")]
		public string ProductTaxCode { get; set; }

		[JsonProperty("unit_price")]
		public decimal UnitPrice { get; set; }

		[JsonProperty("discount")]
		public decimal Discount { get; set; }

		[JsonProperty("sales_tax")]
		public decimal SalesTax { get; set; }
	}
}
