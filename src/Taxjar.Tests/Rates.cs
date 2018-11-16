using Newtonsoft.Json;
using NUnit.Framework;
using System.Threading.Tasks;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;

namespace Taxjar.Tests
{
	[TestFixture]
    public class RatesTests
	{
		[Test]
		public void when_showing_tax_rates_for_a_location()
		{
            var body = JsonConvert.DeserializeObject<RateResponse>(TaxjarFixture.GetJSON("rates.json"));

            Bootstrap.server.Given(
                Request.Create()
                    .WithPath("/v2/rates/90002")
                    .UsingGet()
            ).RespondWith(
                Response.Create()
                    .WithStatusCode(200)
                    .WithHeader("Content-Type", "application/json")
                    .WithBodyAsJson(body)
            );

			var rates = Bootstrap.client.RatesForLocation("90002");

			Assert.AreEqual("90002", rates.Zip);
			Assert.AreEqual("CA", rates.State);
			Assert.AreEqual(0.065, rates.StateRate);
			Assert.AreEqual("LOS ANGELES", rates.County);
			Assert.AreEqual(0.01, rates.CountyRate);
			Assert.AreEqual("WATTS", rates.City);
			Assert.AreEqual(0, rates.CityRate);
			Assert.AreEqual(0.015, rates.CombinedDistrictRate);
			Assert.AreEqual(0.09, rates.CombinedRate);
            Assert.AreEqual(false, rates.FreightTaxable);
		}

        [Test]
        public async Task when_showing_tax_rates_for_a_location_async()
        {
            var body = JsonConvert.DeserializeObject<RateResponse>(TaxjarFixture.GetJSON("rates.json"));

            Bootstrap.server.Given(
                Request.Create()
                    .WithPath("/v2/rates/90002")
                    .UsingGet()
            ).RespondWith(
                Response.Create()
                    .WithStatusCode(200)
                    .WithHeader("Content-Type", "application/json")
                    .WithBodyAsJson(body)
            );

            var rates = await Bootstrap.client.RatesForLocationAsync("90002");

            Assert.AreEqual("90002", rates.Zip);
            Assert.AreEqual("CA", rates.State);
            Assert.AreEqual(0.065, rates.StateRate);
            Assert.AreEqual("LOS ANGELES", rates.County);
            Assert.AreEqual(0.01, rates.CountyRate);
            Assert.AreEqual("WATTS", rates.City);
            Assert.AreEqual(0, rates.CityRate);
            Assert.AreEqual(0.015, rates.CombinedDistrictRate);
            Assert.AreEqual(0.09, rates.CombinedRate);
            Assert.AreEqual(false, rates.FreightTaxable);
        }

        [Test]
		public void when_showing_tax_rates_for_a_location_sst()
		{
            var body = JsonConvert.DeserializeObject<RateResponse>(TaxjarFixture.GetJSON("rates_sst.json"));

            Bootstrap.server.Given(
                Request.Create()
                    .WithPath("/v2/rates/05495-2086")
                    .UsingGet()
            ).RespondWith(
                Response.Create()
                    .WithStatusCode(200)
                    .WithHeader("Content-Type", "application/json")
                    .WithBodyAsJson(body)
            );

			var rates = Bootstrap.client.RatesForLocation("05495-2086");

			Assert.AreEqual("05495-2086", rates.Zip);
            Assert.AreEqual("US", rates.Country);
            Assert.AreEqual(0, rates.CountryRate);
			Assert.AreEqual("VT", rates.State);
			Assert.AreEqual(0.06, rates.StateRate);
			Assert.AreEqual("CHITTENDEN", rates.County);
			Assert.AreEqual(0, rates.CountyRate);
			Assert.AreEqual("WILLISTON", rates.City);
			Assert.AreEqual(0, rates.CityRate);
			Assert.AreEqual(0.01, rates.CombinedDistrictRate);
			Assert.AreEqual(0.07, rates.CombinedRate);
			Assert.AreEqual(true, rates.FreightTaxable);
		}

		[Test]
		public void when_showing_tax_rates_for_a_location_ca()
		{
            var body = JsonConvert.DeserializeObject<RateResponse>(TaxjarFixture.GetJSON("rates_ca.json"));

            Bootstrap.server.Given(
                Request.Create()
                    .WithPath("/v2/rates/V5K0A1")
                    .UsingGet()
            ).RespondWith(
                Response.Create()
                    .WithStatusCode(200)
                    .WithHeader("Content-Type", "application/json")
                    .WithBodyAsJson(body)
            );

			var rates = Bootstrap.client.RatesForLocation("V5K0A1");

			Assert.AreEqual("V5K0A1", rates.Zip);
            Assert.AreEqual("Vancouver", rates.City);
            Assert.AreEqual("BC", rates.State);
			Assert.AreEqual("CA", rates.Country);
            Assert.AreEqual(0.12, rates.CombinedRate);
            Assert.AreEqual(true, rates.FreightTaxable);
		}

		[Test]
		public void when_showing_tax_rates_for_a_location_au()
		{
            var body = JsonConvert.DeserializeObject<RateResponse>(TaxjarFixture.GetJSON("rates_au.json"));

            Bootstrap.server.Given(
                Request.Create()
                    .WithPath("/v2/rates/2060")
                    .UsingGet()
            ).RespondWith(
                Response.Create()
                    .WithStatusCode(200)
                    .WithHeader("Content-Type", "application/json")
                    .WithBodyAsJson(body)
            );

			var rates = Bootstrap.client.RatesForLocation("2060");

			Assert.AreEqual("2060", rates.Zip);
			Assert.AreEqual("AU", rates.Country);
            Assert.AreEqual(0.1, rates.CountryRate);
			Assert.AreEqual(0.1, rates.CombinedRate);
			Assert.AreEqual(true, rates.FreightTaxable);
		}

		[Test]
		public void when_showing_tax_rates_for_a_location_eu()
		{
            var body = JsonConvert.DeserializeObject<RateResponse>(TaxjarFixture.GetJSON("rates_eu.json"));

            Bootstrap.server.Given(
                Request.Create()
                    .WithPath("/v2/rates/00150")
                    .UsingGet()
            ).RespondWith(
                Response.Create()
                    .WithStatusCode(200)
                    .WithHeader("Content-Type", "application/json")
                    .WithBodyAsJson(body)
            );

			var rates = Bootstrap.client.RatesForLocation("00150");

			Assert.AreEqual("FI", rates.Country);
            Assert.AreEqual("Finland", rates.Name);
            Assert.AreEqual(0.24, rates.StandardRate);
            Assert.AreEqual(0, rates.ReducedRate);
            Assert.AreEqual(0, rates.SuperReducedRate);
            Assert.AreEqual(0, rates.ParkingRate);
            Assert.AreEqual(0, rates.DistanceSaleThreshold);
            Assert.AreEqual(true, rates.FreightTaxable);
		}
	}
}