using System.Collections.Generic;
using Newtonsoft.Json;

namespace Taxjar
{
    public class SummaryRatesResponse
    {
        [JsonProperty("summary_rates")]
        public List<SummaryRate> SummaryRates { get; set; }
    }

    public class SummaryRate
    {
        [JsonProperty("country_code")]
        public string CountryCode { get; set; }

        [JsonProperty("country")]
        public string Country { get; set; }

        [JsonProperty("region_code")]
        public string RegionCode { get; set; }

        [JsonProperty("region")]
        public string Region { get; set; }

        [JsonProperty("minimum_rate")]
        public SummaryRateObject MinimumRate { get; set; }

        [JsonProperty("average_rate")]
        public SummaryRateObject AverageRate { get; set; }
    }

    public class SummaryRateObject
    {
        [JsonProperty("label")]
        public string Label { get; set; }

        [JsonProperty("rate")]
        public decimal Rate { get; set; }
    }
}
