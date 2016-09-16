using HttpMock;
using NUnit.Framework;

namespace Taxjar.Tests
{
	[TestFixture]
	public class RatesTests
	{
		internal TaxjarApi client;

		[SetUp]
		public void Init()
		{
			this.client = new TaxjarApi("foo123", new { apiUrl = "http://localhost:9191/v2/" });
		}

		[Test]
		public void when_showing_tax_rates_for_a_location()
		{
			var stubHttp = HttpMockRepository.At("http://localhost:9191");

			stubHttp.Stub(x => x.Get("/v2/rates/90002"))
					.Return(TaxjarFixture.GetJSON("rates.json"))
					.OK();

			var rates = client.RatesForLocation("90002");

			Assert.AreEqual("90002", rates.Zip);
			Assert.AreEqual("CA", rates.State);
			Assert.AreEqual(0.065, rates.StateRate);
			Assert.AreEqual("LOS ANGELES", rates.County);
			Assert.AreEqual(0.01, rates.CountyRate);
			Assert.AreEqual("WATTS", rates.City);
			Assert.AreEqual(0, rates.CityRate);
			Assert.AreEqual(0.015, rates.CombinedDistrictRate);
			Assert.AreEqual(0.09, rates.CombinedRate);
		}
	}
}