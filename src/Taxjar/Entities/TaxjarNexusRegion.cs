using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Taxjar
{
    public record NexusRegionsResponse
    {
        [JsonPropertyName("regions")]
        public List<NexusRegion>? Regions { get; set; }
    }

    public record NexusRegion
    {
        ///<summary>
        ///Two-letter ISO country code for nexus region.
        ///</summary>
        [JsonPropertyName("country_code")]
        public string? CountryCode { get; set; }

        ///<summary>
        ///Country name for nexus region.
        ///</summary>
        [JsonPropertyName("country")]
        public string? Country { get; set; }

        ///<summary>
        ///Two-letter ISO region code for nexus region.
        ///</summary>
        [JsonPropertyName("region_code")]
        public string? RegionCode { get; set; }

        ///<summary>
        ///Region name for nexus region.
        ///</summary>
        [JsonPropertyName("region")]
        public string? Region { get; set; }
    }
}
