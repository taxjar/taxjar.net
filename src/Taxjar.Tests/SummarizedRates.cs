using Newtonsoft.Json;
using NUnit.Framework;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;

namespace Taxjar.Tests
{
	[TestFixture]
	public class SummarizedRateTests
	{
		[Test]
		public void when_summarizing_tax_rates_for_all_regions()
		{
            var body = JsonConvert.DeserializeObject<SummaryRatesResponse>(TaxjarFixture.GetJSON("summary_rates.json"));

            Bootstrap.server.Given(
                Request.Create()
                    .WithPath("/v2/summary_rates")
                    .UsingGet()
            ).RespondWith(
                Response.Create()
                    .WithStatusCode(200)
                    .WithBodyAsJson(body)
            );

            var summaryRates = Bootstrap.client.SummaryRates();

			Assert.AreEqual(3, summaryRates.Count);
			Assert.AreEqual("US", summaryRates[0].CountryCode);
			Assert.AreEqual("United States", summaryRates[0].Country);
			Assert.AreEqual("CA", summaryRates[0].RegionCode);
			Assert.AreEqual("California", summaryRates[0].Region);
			Assert.AreEqual("State Tax", summaryRates[0].MinimumRate.Label);
			Assert.AreEqual(0.065, summaryRates[0].MinimumRate.Rate);
			Assert.AreEqual("Tax", summaryRates[0].AverageRate.Label);
			Assert.AreEqual(0.0827, summaryRates[0].AverageRate.Rate);
		}
	}
}