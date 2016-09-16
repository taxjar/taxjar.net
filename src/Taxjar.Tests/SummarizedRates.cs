using System;
using System.Net;
using System.IO;
using Taxjar;
using HttpMock;
using NUnit.Framework;

namespace Taxjar.Tests
{
	[TestFixture]
	public class SummarizedRateTests
	{
		internal TaxjarApi client;

		[SetUp]
		public void Init()
		{
			this.client = new TaxjarApi("foo123", new { apiUrl = "http://localhost:9191/v2/" });
		}

		[Test]
		public void when_summarizing_tax_rates_for_all_regions()
		{
			var stubHttp = HttpMockRepository.At("http://localhost:9191");

			stubHttp.Stub(x => x.Get("/v2/summary_rates"))
					.Return(TaxjarFixture.GetJSON("summary_rates.json"))
					.OK();

			var summaryRates = client.SummaryRates();

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