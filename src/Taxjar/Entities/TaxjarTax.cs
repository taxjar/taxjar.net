using System.Collections.Generic;
using Newtonsoft.Json;

namespace Taxjar
{
	public class TaxResponse
	{
		[JsonProperty("tax")]
        public TaxResponseAttributes Tax { get; set; }
	}

	public class TaxResponseAttributes
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

    public class Tax
    {
        [JsonProperty("from_country")]
        public string FromCountry { get; set; }

        [JsonProperty("from_zip")]
        public string FromZip { get; set; }

        [JsonProperty("from_state")]
        public string FromState { get; set; }

        [JsonProperty("from_city")]
        public string FromCity { get; set; }

        [JsonProperty("from_street")]
        public string FromStreet { get; set; }

        [JsonProperty("to_country")]
        public string ToCountry { get; set; }

        [JsonProperty("to_zip")]
        public string ToZip { get; set; }

        [JsonProperty("to_state")]
        public string ToState { get; set; }

        [JsonProperty("to_city")]
        public string ToCity { get; set; }

        [JsonProperty("to_street")]
        public string ToStreet { get; set; }

        [JsonProperty("amount", NullValueHandling = NullValueHandling.Ignore)]
        public decimal Amount { get; set; }

        [JsonProperty("shipping", NullValueHandling = NullValueHandling.Ignore)]
        public decimal Shipping { get; set; }

        [JsonProperty("nexus_addresses")]
        public List<NexusAddress> NexusAddresses { get; set; }

        [JsonProperty("line_items")]
        public List<TaxLineItem> LineItems { get; set; }
    }

    public class TaxLineItem
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("quantity")]
        public int Quantity { get; set; }

        [JsonProperty("product_tax_code")]
        public string ProductTaxCode { get; set; }

        [JsonProperty("unit_price")]
        public decimal UnitPrice { get; set; }

        [JsonProperty("discount")]
        public decimal Discount { get; set; }
    }
}
