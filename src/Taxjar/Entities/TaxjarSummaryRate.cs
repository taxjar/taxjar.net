using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Taxjar
{
    ///<summary>
    ///List of summarized rates for each region/state
    ///</summary>
    public record SummaryRatesResponse
    {

        [JsonPropertyName("summary_rates")]
        public List<SummaryRate> SummaryRates { get; set; } = new List<SummaryRate>();
    }

    public record SummaryRate
    {
        ///<summary>
        ///Region code for summarized region.
        ///</summary>
        [JsonPropertyName("country_code")]
        public string CountryCode { get; set; } = string.Empty;

        ///<summary>
        ///Country name for summarized region.
        ///</summary>
        [JsonPropertyName("country")]
        public string Country { get; set; } = string.Empty;

        ///<summary>
        ///Region code for summarized region.
        ///</summary>
        [JsonPropertyName("region_code")]
        public string RegionCode { get; set; } = string.Empty;

        ///<summary>
        ///Region name for summarized region.
        ///</summary>
        [JsonPropertyName("region")]
        public string Region { get; set; } = string.Empty;

        ///<summary>
        ///Region/state-only sales tax rate with label.
        ///</summary>
        [JsonPropertyName("minimum_rate")]
        public SummaryRateObject? MinimumRate { get; set; }

        ///<summary>
        ///Average rate for region/state and local sales tax across all postal codes in the summarized region with label.
        ///</summary>
        [JsonPropertyName("average_rate")]
        public SummaryRateObject? AverageRate { get; set; }
    }

    public record SummaryRateObject
    {

        [JsonPropertyName("label")]
        public string Label { get; set; } = string.Empty;

        [JsonPropertyName("rate")]
        public decimal Rate { get; set; }
    }
}
