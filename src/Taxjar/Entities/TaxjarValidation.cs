using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Taxjar
{
	public class ValidationResponse
	{
		[JsonProperty("validation")]
        public ValidationResponseAttributes Validation { get; set; }
	}

	public class ValidationResponseAttributes
	{
		[JsonProperty("valid")]
		public bool Valid { get; set; }

		[JsonProperty("exists")]
		public bool Exists { get; set; }

		[JsonProperty("vies_available")]
		public bool ViesAvailable { get; set; }

		[JsonProperty("vies_response")]
		public ViesResponse ViesResponse { get; set; }
	}

	public class ViesResponse
	{
		[JsonProperty("country_code")]
		public string CountryCode { get; set; }

		[JsonProperty("vat_number")]
		public string VatNumber { get; set; }

		[JsonProperty("request_date")]
		public string RequestDate { get; set; }

		[JsonProperty("valid")]
		public bool Valid { get; set; }

		[JsonProperty("name")]
		public string Name { get; set; }

		[JsonProperty("address")]
		public string Address { get; set; }
	}

    public class Validation
    {
        [JsonProperty("vat")]
        public string Vat { get; set; }
    }
}
