using Newtonsoft.Json;
using NUnit.Framework;
using System.Threading.Tasks;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;

namespace Taxjar.Tests
{
	[TestFixture]
    public class NexusTests
	{
		[Test]
		public void when_listing_nexus_regions()
		{
            var body = JsonConvert.DeserializeObject<NexusRegionsResponse>(TaxjarFixture.GetJSON("nexus_regions.json"));

            Bootstrap.server.Given(
                Request.Create()
                    .WithPath("/v2/nexus/regions")
                    .UsingGet()
            ).RespondWith(
                Response.Create()
                    .WithStatusCode(200)
                    .WithHeader("Content-Type", "application/json")
                    .WithBodyAsJson(body)
            );

            var nexusRegions = Bootstrap.client.NexusRegions();

			Assert.AreEqual(3, nexusRegions.Count);
			Assert.AreEqual("US", nexusRegions[0].CountryCode);
			Assert.AreEqual("United States", nexusRegions[0].Country);
			Assert.AreEqual("CA", nexusRegions[0].RegionCode);
			Assert.AreEqual("California", nexusRegions[0].Region);
		}

        [Test]
        public async Task when_listing_nexus_regions_async()
        {
            var body = JsonConvert.DeserializeObject<NexusRegionsResponse>(TaxjarFixture.GetJSON("nexus_regions.json"));

            Bootstrap.server.Given(
                Request.Create()
                    .WithPath("/v2/nexus/regions")
                    .UsingGet()
            ).RespondWith(
                Response.Create()
                    .WithStatusCode(200)
                    .WithHeader("Content-Type", "application/json")
                    .WithBodyAsJson(body)
            );

            var nexusRegionsRequest = await Bootstrap.client.NexusRegionsAsync();
            var nexusRegions = nexusRegionsRequest.Regions;

            Assert.AreEqual(3, nexusRegions.Count);
            Assert.AreEqual("US", nexusRegions[0].CountryCode);
            Assert.AreEqual("United States", nexusRegions[0].Country);
            Assert.AreEqual("CA", nexusRegions[0].RegionCode);
            Assert.AreEqual("California", nexusRegions[0].Region);
        }
    }
}