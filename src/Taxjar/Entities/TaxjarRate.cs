using Newtonsoft.Json;

namespace Taxjar
{
	public class RateResponse
	{
		[JsonProperty("rate")]
        public RateResponseAttributes Rate { get; set; }
	}

	public class RateResponseAttributes
	{
		[JsonProperty("zip")]
		public string Zip { get; set; }

		[JsonProperty("state")]
		public string State { get; set; }

		[JsonProperty("state_rate")]
		public decimal StateRate { get; set; }

		[JsonProperty("county")]
		public string County { get; set; }

		[JsonProperty("county_rate")]
		public decimal CountyRate { get; set; }

		[JsonProperty("city")]
		public string City { get; set; }

		[JsonProperty("city_rate")]
		public decimal CityRate { get; set; }

		[JsonProperty("combined_district_rate")]
		public decimal CombinedDistrictRate { get; set; }

		[JsonProperty("combined_rate")]
		public decimal CombinedRate { get; set; }

		[JsonProperty("freight_taxable")]
		public bool FreightTaxable { get; set; }

		// International
		[JsonProperty("country")]
		public string Country { get; set; }

		[JsonProperty("name")]
		public string Name { get; set; }

		// Australia / SST States
		[JsonProperty("country_rate")]
		public decimal CountryRate { get; set; }

		// European Union
		[JsonProperty("standard_rate")]
		public decimal StandardRate { get; set; }

		[JsonProperty("reduced_rate", NullValueHandling = NullValueHandling.Ignore)]
		public decimal ReducedRate { get; set; }

		[JsonProperty("super_reduced_rate", NullValueHandling = NullValueHandling.Ignore)]
		public decimal SuperReducedRate { get; set; }

		[JsonProperty("parking_rate", NullValueHandling = NullValueHandling.Ignore)]
		public decimal ParkingRate { get; set; }

		[JsonProperty("distance_sale_threshold", NullValueHandling = NullValueHandling.Ignore)]
		public decimal DistanceSaleThreshold { get; set; }
	}

    public class Rate
    {
        [JsonProperty("country")]
        public string Country { get; set; }

        [JsonProperty("zip")]
        public string Zip { get; set; }

        [JsonProperty("state")]
        public string State { get; set; }

        [JsonProperty("city")]
        public string City { get; set; }

        [JsonProperty("street")]
        public string Street { get; set; }
    }
}
