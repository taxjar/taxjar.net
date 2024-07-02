using System.Text.Json.Serialization;

namespace Taxjar
{
    public record RateResponse
    {
        [JsonPropertyName("rate")]
        public RateResponseAttributes? Rate { get; set; }
    }

    public record RateResponseAttributes
    {
        [JsonPropertyName("zip")]
        public string Zip { get; set; } = string.Empty;

        [JsonPropertyName("state")]
        public string State { get; set; } = string.Empty;

        [JsonPropertyName("state_rate")]
        public decimal StateRate { get; set; }

        [JsonPropertyName("county")]
        public string County { get; set; } = string.Empty;

        [JsonPropertyName("county_rate")]
        public decimal CountyRate { get; set; }

        [JsonPropertyName("city")]
        public string City { get; set; } = string.Empty;

        [JsonPropertyName("city_rate")]
        public decimal CityRate { get; set; }

        [JsonPropertyName("combined_district_rate")]
        public decimal CombinedDistrictRate { get; set; }

        [JsonPropertyName("combined_rate")]
        public decimal CombinedRate { get; set; }

        [JsonPropertyName("freight_taxable")]
        public bool FreightTaxable { get; set; }

        // International
        [JsonPropertyName("country")]
        public string Country { get; set; } = string.Empty;

        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        // Australia / SST States
        [JsonPropertyName("country_rate")]
        public decimal CountryRate { get; set; }

        // European Union
        [JsonPropertyName("standard_rate")]
        public decimal StandardRate { get; set; }

        [JsonPropertyName("reduced_rate")]
        public decimal ReducedRate { get; set; }

        [JsonPropertyName("super_reduced_rate")]
        public decimal SuperReducedRate { get; set; }

        [JsonPropertyName("parking_rate")]
        public decimal ParkingRate { get; set; }

        [JsonPropertyName("distance_sale_threshold")]
        public decimal DistanceSaleThreshold { get; set; }
    }

    public record Rate
    {
        [JsonPropertyName("country")]
        public string Country { get; set; } = string.Empty;

        [JsonPropertyName("zip")]
        public string Zip { get; set; } = string.Empty;

        [JsonPropertyName("state")]
        public string State { get; set; } = string.Empty;

        [JsonPropertyName("city")]
        public string City { get; set; } = string.Empty;

        [JsonPropertyName("street")]
        public string Street { get; set; } = string.Empty;
    }
}
