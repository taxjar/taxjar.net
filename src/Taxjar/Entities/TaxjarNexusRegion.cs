using System.Collections.Generic;
using Newtonsoft.Json;

namespace Taxjar
{
	public class NexusRegionsResponse
	{
		[JsonProperty("regions")]
        public List<NexusRegion> Regions { get; set; }
	}

	public class NexusRegion
	{
		[JsonProperty("country_code")]
		public string CountryCode { get; set; }

		[JsonProperty("country")]
		public string Country { get; set; }

		[JsonProperty("region_code")]
		public string RegionCode { get; set; }

		[JsonProperty("region")]
		public string Region { get; set; }
	}
}
