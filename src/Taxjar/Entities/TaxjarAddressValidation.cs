using System.Collections.Generic;
using Newtonsoft.Json;

namespace Taxjar
{
    public class AddressValidationResponse
    {
        [JsonProperty("addresses")]
        public List<Address> Addresses { get; set; }
    }

    public class Address
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
