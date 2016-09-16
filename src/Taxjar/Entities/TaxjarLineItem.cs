using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Taxjar
{
	public class LineItem
	{
		[JsonProperty("id")]
		public int Id { get; set; }

		[JsonProperty("quantity")]
		public int Quantity { get; set; }

		[JsonProperty("product_identifier")]
		public string ProductIdentifier { get; set; }

		[JsonProperty("description")]
		public string Description { get; set; }

		[JsonProperty("unit_price")]
		public decimal UnitPrice { get; set; }

		[JsonProperty("discount")]
		public decimal Discount { get; set; }

		[JsonProperty("sales_tax")]
		public decimal SalesTax { get; set; }
	}
}
