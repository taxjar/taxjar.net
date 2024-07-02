using System.Text.Json.Serialization;

namespace Taxjar
{
    public record NexusAddress
    {
        ///<summary>
        ///Unique identifier of the given nexus address.
        ///</summary>
        ///<remarks>
        ///Unique identifier of the given nexus address.
        ///</remarks>
        [JsonPropertyName("id")]
        public string? Id { get; set; }

        ///<summary>
        ///Two-letter ISO country code for the nexus address.
        ///</summary>
        [JsonPropertyName("country")]
        public string? Country { get; set; }

        ///<summary>
        ///Postal code for the nexus address.
        ///</summary>
        [JsonPropertyName("zip")]
        public string? Zip { get; set; }

        ///<summary>
        ///Two-letter ISO state code for the nexus address.
        ///</summary>
        [JsonPropertyName("state")]
        public string? State { get; set; }

        ///<summary>
        ///City for the nexus address.
        ///</summary>
        [JsonPropertyName("city")]
        public string? City { get; set; }

        ///<summary>
        ///Street address for the nexus address.
        ///</summary>
        [JsonPropertyName("street")]
        public string? Street { get; set; }
    }
}
