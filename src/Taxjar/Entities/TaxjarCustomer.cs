using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Taxjar
{
    public class CustomersResponse
    {
        [JsonProperty("customers")]
        public List<string> Customers { get; set; }
    }

    public class CustomerResponse
    {
        [JsonProperty("customer")]
        public CustomerResponseAttributes Customer { get; set; }
    }

    public class CustomerResponseAttributes : Customer
    {
    }

    public class Customer
    {
        [JsonProperty("customer_id")]
        public string CustomerId { get; set; }

        [JsonProperty("exemption_type")]
        public string ExemptionType { get; set; }

        [JsonProperty("exempt_regions")]
        public List<ExemptRegion> ExemptRegions { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("country")]
        public string Country { get; set; }

        [JsonProperty("state")]
        public string State { get; set; }

        [JsonProperty("zip")]
        public string Zip { get; set; }

        [JsonProperty("city")]
        public string City { get; set; }

        [JsonProperty("street")]
        public string Street { get; set; }
    }

    public class ExemptRegion
    {
        [JsonProperty("country")]
        public string Country { get; set; }

        [JsonProperty("state")]
        public string State { get; set; }
    }
}
