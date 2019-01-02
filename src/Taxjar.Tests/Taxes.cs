using Newtonsoft.Json;
using NUnit.Framework;
using System.Threading.Tasks;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;

namespace Taxjar.Tests
{
	[TestFixture]
	public class TaxesTests
	{
        [SetUp]
        public static void Init()
        {
            Bootstrap.client = new TaxjarApi(Bootstrap.apiKey, new { apiUrl = "http://localhost:9191" });
            Bootstrap.server.ResetMappings();
        }

		[Test]
		public void when_calculating_sales_tax_for_an_order()
		{
            var body = JsonConvert.DeserializeObject<TaxResponse>(TaxjarFixture.GetJSON("taxes.json"));

            Bootstrap.server.Given(
                Request.Create()
                    .WithPath("/v2/taxes")
                    .UsingPost()
            ).RespondWith(
                Response.Create()
                    .WithStatusCode(200)
                    .WithHeader("Content-Type", "application/json")
                    .WithBodyAsJson(body)
            );

			var rates = Bootstrap.client.TaxForOrder(new {
				from_country =  "US",
				from_zip = "12207",
				from_state = "NY",
				to_country = "US",
				to_zip = "10001",
				to_state = "NY",
				amount = 60,
				shipping = 10,
				line_items = new[] {
					new
					{
						quantity = 1,
						unit_price = 50
					}
				}
			});

			Assert.AreEqual(60, rates.OrderTotalAmount);
			Assert.AreEqual(10, rates.Shipping);
			Assert.AreEqual(60, rates.TaxableAmount);
			Assert.AreEqual(6.53, rates.AmountToCollect);
			Assert.AreEqual(0.10875, rates.Rate);
			Assert.AreEqual(true, rates.HasNexus);
			Assert.AreEqual(true, rates.FreightTaxable);
			Assert.AreEqual("destination", rates.TaxSource);

            // Jurisdictions
            Assert.AreEqual("US", rates.Jurisdictions.Country);
            Assert.AreEqual("NY", rates.Jurisdictions.State);
            Assert.AreEqual("NEW YORK", rates.Jurisdictions.County);
            Assert.AreEqual("NEW YORK", rates.Jurisdictions.City);

            // Breakdowns
            Assert.AreEqual(10, rates.Breakdown.Shipping.TaxableAmount);
			Assert.AreEqual(0.89, rates.Breakdown.Shipping.TaxCollectable);
			Assert.AreEqual(0.10875, rates.Breakdown.Shipping.CombinedTaxRate);
			Assert.AreEqual(10, rates.Breakdown.Shipping.StateTaxableAmount);
			Assert.AreEqual(0.4, rates.Breakdown.Shipping.StateAmount);
			Assert.AreEqual(0.04, rates.Breakdown.Shipping.StateSalesTaxRate);
			Assert.AreEqual(10, rates.Breakdown.Shipping.CountyTaxableAmount);
			Assert.AreEqual(0.1, rates.Breakdown.Shipping.CountyAmount);
			Assert.AreEqual(0.01, rates.Breakdown.Shipping.CountyTaxRate);
			Assert.AreEqual(10, rates.Breakdown.Shipping.CityTaxableAmount);
			Assert.AreEqual(0.49, rates.Breakdown.Shipping.CityAmount);
			Assert.AreEqual(0.04875, rates.Breakdown.Shipping.CityTaxRate);
            Assert.AreEqual(10, rates.Breakdown.Shipping.SpecialDistrictTaxableAmount);
            Assert.AreEqual(0.01, rates.Breakdown.Shipping.SpecialDistrictTaxRate);
            Assert.AreEqual(0.1, rates.Breakdown.Shipping.SpecialDistrictAmount);

			Assert.AreEqual(60, rates.Breakdown.TaxableAmount);
			Assert.AreEqual(6.53, rates.Breakdown.TaxCollectable);
			Assert.AreEqual(0.10875, rates.Breakdown.CombinedTaxRate);
			Assert.AreEqual(60, rates.Breakdown.StateTaxableAmount);
			Assert.AreEqual(0.04, rates.Breakdown.StateTaxRate);
			Assert.AreEqual(2.4, rates.Breakdown.StateTaxCollectable);
			Assert.AreEqual(60, rates.Breakdown.CountyTaxableAmount);
			Assert.AreEqual(0.01, rates.Breakdown.CountyTaxRate);
			Assert.AreEqual(0.6, rates.Breakdown.CountyTaxCollectable);
			Assert.AreEqual(60, rates.Breakdown.CityTaxableAmount);
			Assert.AreEqual(0.04875, rates.Breakdown.CityTaxRate);
			Assert.AreEqual(2.93, rates.Breakdown.CityTaxCollectable);
			Assert.AreEqual(60, rates.Breakdown.SpecialDistrictTaxableAmount);
			Assert.AreEqual(0.01, rates.Breakdown.SpecialDistrictTaxRate);
			Assert.AreEqual(0.6, rates.Breakdown.SpecialDistrictTaxCollectable);

			// Line Items
			Assert.AreEqual("1", rates.Breakdown.LineItems[0].Id);
			Assert.AreEqual(50, rates.Breakdown.LineItems[0].TaxableAmount);
			Assert.AreEqual(4.44, rates.Breakdown.LineItems[0].TaxCollectable);
			Assert.AreEqual(0.10875, rates.Breakdown.LineItems[0].CombinedTaxRate);
			Assert.AreEqual(50, rates.Breakdown.LineItems[0].StateTaxableAmount);
			Assert.AreEqual(0.04, rates.Breakdown.LineItems[0].StateSalesTaxRate);
			Assert.AreEqual(2, rates.Breakdown.LineItems[0].StateAmount);
			Assert.AreEqual(50, rates.Breakdown.LineItems[0].CountyTaxableAmount);
			Assert.AreEqual(0.01, rates.Breakdown.LineItems[0].CountyTaxRate);
			Assert.AreEqual(0.5, rates.Breakdown.LineItems[0].CountyAmount);
			Assert.AreEqual(50, rates.Breakdown.LineItems[0].CityTaxableAmount);
			Assert.AreEqual(0.04875, rates.Breakdown.LineItems[0].CityTaxRate);
			Assert.AreEqual(2.44, rates.Breakdown.LineItems[0].CityAmount);
			Assert.AreEqual(50, rates.Breakdown.LineItems[0].SpecialDistrictTaxableAmount);
			Assert.AreEqual(0.01, rates.Breakdown.LineItems[0].SpecialTaxRate);
			Assert.AreEqual(0.5, rates.Breakdown.LineItems[0].SpecialDistrictAmount);
		}

        [Test]
        public async Task when_calculating_sales_tax_for_an_order_async()
        {
            var body = JsonConvert.DeserializeObject<TaxResponse>(TaxjarFixture.GetJSON("taxes.json"));

            Bootstrap.server.Given(
                Request.Create()
                    .WithPath("/v2/taxes")
                    .UsingPost()
            ).RespondWith(
                Response.Create()
                    .WithStatusCode(200)
                    .WithHeader("Content-Type", "application/json")
                    .WithBodyAsJson(body)
            );

            var rates = await Bootstrap.client.TaxForOrderAsync(new
            {
                from_country = "US",
                from_zip = "12207",
                from_state = "NY",
                to_country = "US",
                to_zip = "10001",
                to_state = "NY",
                amount = 60,
                shipping = 10,
                line_items = new[] {
                    new
                    {
                        quantity = 1,
                        unit_price = 50
                    }
                }
            });

            Assert.AreEqual(60, rates.OrderTotalAmount);
            Assert.AreEqual(10, rates.Shipping);
            Assert.AreEqual(60, rates.TaxableAmount);
            Assert.AreEqual(6.53, rates.AmountToCollect);
            Assert.AreEqual(0.10875, rates.Rate);
            Assert.AreEqual(true, rates.HasNexus);
            Assert.AreEqual(true, rates.FreightTaxable);
            Assert.AreEqual("destination", rates.TaxSource);

            // Jurisdictions
            Assert.AreEqual("US", rates.Jurisdictions.Country);
            Assert.AreEqual("NY", rates.Jurisdictions.State);
            Assert.AreEqual("NEW YORK", rates.Jurisdictions.County);
            Assert.AreEqual("NEW YORK", rates.Jurisdictions.City);

            // Breakdowns
            Assert.AreEqual(10, rates.Breakdown.Shipping.TaxableAmount);
            Assert.AreEqual(0.89, rates.Breakdown.Shipping.TaxCollectable);
            Assert.AreEqual(0.10875, rates.Breakdown.Shipping.CombinedTaxRate);
            Assert.AreEqual(10, rates.Breakdown.Shipping.StateTaxableAmount);
            Assert.AreEqual(0.4, rates.Breakdown.Shipping.StateAmount);
            Assert.AreEqual(0.04, rates.Breakdown.Shipping.StateSalesTaxRate);
            Assert.AreEqual(10, rates.Breakdown.Shipping.CountyTaxableAmount);
            Assert.AreEqual(0.1, rates.Breakdown.Shipping.CountyAmount);
            Assert.AreEqual(0.01, rates.Breakdown.Shipping.CountyTaxRate);
            Assert.AreEqual(10, rates.Breakdown.Shipping.CityTaxableAmount);
            Assert.AreEqual(0.49, rates.Breakdown.Shipping.CityAmount);
            Assert.AreEqual(0.04875, rates.Breakdown.Shipping.CityTaxRate);
            Assert.AreEqual(10, rates.Breakdown.Shipping.SpecialDistrictTaxableAmount);
            Assert.AreEqual(0.01, rates.Breakdown.Shipping.SpecialDistrictTaxRate);
            Assert.AreEqual(0.1, rates.Breakdown.Shipping.SpecialDistrictAmount);

            Assert.AreEqual(60, rates.Breakdown.TaxableAmount);
            Assert.AreEqual(6.53, rates.Breakdown.TaxCollectable);
            Assert.AreEqual(0.10875, rates.Breakdown.CombinedTaxRate);
            Assert.AreEqual(60, rates.Breakdown.StateTaxableAmount);
            Assert.AreEqual(0.04, rates.Breakdown.StateTaxRate);
            Assert.AreEqual(2.4, rates.Breakdown.StateTaxCollectable);
            Assert.AreEqual(60, rates.Breakdown.CountyTaxableAmount);
            Assert.AreEqual(0.01, rates.Breakdown.CountyTaxRate);
            Assert.AreEqual(0.6, rates.Breakdown.CountyTaxCollectable);
            Assert.AreEqual(60, rates.Breakdown.CityTaxableAmount);
            Assert.AreEqual(0.04875, rates.Breakdown.CityTaxRate);
            Assert.AreEqual(2.93, rates.Breakdown.CityTaxCollectable);
            Assert.AreEqual(60, rates.Breakdown.SpecialDistrictTaxableAmount);
            Assert.AreEqual(0.01, rates.Breakdown.SpecialDistrictTaxRate);
            Assert.AreEqual(0.6, rates.Breakdown.SpecialDistrictTaxCollectable);

            // Line Items
            Assert.AreEqual("1", rates.Breakdown.LineItems[0].Id);
            Assert.AreEqual(50, rates.Breakdown.LineItems[0].TaxableAmount);
            Assert.AreEqual(4.44, rates.Breakdown.LineItems[0].TaxCollectable);
            Assert.AreEqual(0.10875, rates.Breakdown.LineItems[0].CombinedTaxRate);
            Assert.AreEqual(50, rates.Breakdown.LineItems[0].StateTaxableAmount);
            Assert.AreEqual(0.04, rates.Breakdown.LineItems[0].StateSalesTaxRate);
            Assert.AreEqual(2, rates.Breakdown.LineItems[0].StateAmount);
            Assert.AreEqual(50, rates.Breakdown.LineItems[0].CountyTaxableAmount);
            Assert.AreEqual(0.01, rates.Breakdown.LineItems[0].CountyTaxRate);
            Assert.AreEqual(0.5, rates.Breakdown.LineItems[0].CountyAmount);
            Assert.AreEqual(50, rates.Breakdown.LineItems[0].CityTaxableAmount);
            Assert.AreEqual(0.04875, rates.Breakdown.LineItems[0].CityTaxRate);
            Assert.AreEqual(2.44, rates.Breakdown.LineItems[0].CityAmount);
            Assert.AreEqual(50, rates.Breakdown.LineItems[0].SpecialDistrictTaxableAmount);
            Assert.AreEqual(0.01, rates.Breakdown.LineItems[0].SpecialTaxRate);
            Assert.AreEqual(0.5, rates.Breakdown.LineItems[0].SpecialDistrictAmount);
        }

        [Test]
		public void when_calculating_sales_tax_for_an_international_order()
		{
            var body = JsonConvert.DeserializeObject<TaxResponse>(TaxjarFixture.GetJSON("taxes_international.json"));

            Bootstrap.server.Given(
                Request.Create()
                    .WithPath("/v2/taxes")
                    .UsingPost()
            ).RespondWith(
                Response.Create()
                    .WithStatusCode(200)
                    .WithHeader("Content-Type", "application/json")
                    .WithBodyAsJson(body)
            );

			var rates = Bootstrap.client.TaxForOrder(new
			{
				from_country = "FI",
				from_zip = "00150",
				to_country = "FI",
				to_zip = "00150",
				amount = 16.95,
				shipping = 10,
				line_items = new[] {
					new
					{
						quantity = 1,
						unit_price = 16.95
					}
				}
			});

			Assert.AreEqual(26.95, rates.OrderTotalAmount);
			Assert.AreEqual(6.47, rates.AmountToCollect);
			Assert.AreEqual(true, rates.HasNexus);
			Assert.AreEqual(true, rates.FreightTaxable);
			Assert.AreEqual("destination", rates.TaxSource);

            // Jurisdictions
            Assert.AreEqual("FI", rates.Jurisdictions.Country);

            // Breakdowns
            Assert.AreEqual(26.95, rates.Breakdown.TaxableAmount);
			Assert.AreEqual(6.47, rates.Breakdown.TaxCollectable);
			Assert.AreEqual(0.24, rates.Breakdown.CombinedTaxRate);
			Assert.AreEqual(26.95, rates.Breakdown.CountryTaxableAmount);
			Assert.AreEqual(0.24, rates.Breakdown.CountryTaxRate);
			Assert.AreEqual(6.47, rates.Breakdown.CountryTaxCollectable);

			Assert.AreEqual(10, rates.Breakdown.Shipping.TaxableAmount);
			Assert.AreEqual(2.4, rates.Breakdown.Shipping.TaxCollectable);
			Assert.AreEqual(0.24, rates.Breakdown.Shipping.CombinedTaxRate);
			Assert.AreEqual(10, rates.Breakdown.Shipping.CountryTaxableAmount);
			Assert.AreEqual(0.24, rates.Breakdown.Shipping.CountryTaxRate);
			Assert.AreEqual(2.4, rates.Breakdown.Shipping.CountryTaxCollectable);

			// Line Items
			Assert.AreEqual(16.95, rates.Breakdown.LineItems[0].TaxableAmount);
			Assert.AreEqual(4.07, rates.Breakdown.LineItems[0].TaxCollectable);
			Assert.AreEqual(0.24, rates.Breakdown.LineItems[0].CombinedTaxRate);
			Assert.AreEqual(16.95, rates.Breakdown.LineItems[0].CountryTaxableAmount);
			Assert.AreEqual(0.24, rates.Breakdown.LineItems[0].CountryTaxRate);
			Assert.AreEqual(4.07, rates.Breakdown.LineItems[0].CountryTaxCollectable);
		}

		[Test]
		public void when_calculating_sales_tax_for_a_canadian_order()
		{
            var body = JsonConvert.DeserializeObject<TaxResponse>(TaxjarFixture.GetJSON("taxes_canada.json"));

            Bootstrap.server.Given(
                Request.Create()
                    .WithPath("/v2/taxes")
                    .UsingPost()
            ).RespondWith(
                Response.Create()
                    .WithStatusCode(200)
                    .WithHeader("Content-Type", "application/json")
                    .WithBodyAsJson(body)
            );

			var rates = Bootstrap.client.TaxForOrder(new
			{
				from_country = "CA",
				from_zip = "V6G 3E",
				from_state = "BC",
				to_country = "CA",
				to_zip = "M5V 2T6",
				to_state = "ON",
				amount = 16.95,
				shipping = 10,
				line_items = new[] {
					new
					{
						quantity = 1,
						unit_price = 16.95
					}
				}
			});

			Assert.AreEqual(26.95, rates.OrderTotalAmount);
			Assert.AreEqual(10, rates.Shipping);
			Assert.AreEqual(26.95, rates.TaxableAmount);
			Assert.AreEqual(3.5, rates.AmountToCollect);
			Assert.AreEqual(0.13, rates.Rate);
			Assert.AreEqual(true, rates.HasNexus);
			Assert.AreEqual(true, rates.FreightTaxable);
			Assert.AreEqual("destination", rates.TaxSource);

            // Jurisdictions
            Assert.AreEqual("CA", rates.Jurisdictions.Country);
            Assert.AreEqual("ON", rates.Jurisdictions.State);

            // Breakdowns
            Assert.AreEqual(26.95, rates.Breakdown.TaxableAmount);
			Assert.AreEqual(3.5, rates.Breakdown.TaxCollectable);
			Assert.AreEqual(0.13, rates.Breakdown.CombinedTaxRate);
			Assert.AreEqual(26.95, rates.Breakdown.GSTTaxableAmount);
			Assert.AreEqual(0.05, rates.Breakdown.GSTTaxRate);
			Assert.AreEqual(1.35, rates.Breakdown.GST);
			Assert.AreEqual(26.95, rates.Breakdown.PSTTaxableAmount);
			Assert.AreEqual(0.08, rates.Breakdown.PSTTaxRate);
			Assert.AreEqual(2.16, rates.Breakdown.PST);
			Assert.AreEqual(0, rates.Breakdown.QSTTaxableAmount);
			Assert.AreEqual(0, rates.Breakdown.QSTTaxRate);
			Assert.AreEqual(0, rates.Breakdown.QST);

			Assert.AreEqual(10, rates.Breakdown.Shipping.TaxableAmount);
			Assert.AreEqual(1.3, rates.Breakdown.Shipping.TaxCollectable);
			Assert.AreEqual(0.13, rates.Breakdown.Shipping.CombinedTaxRate);
			Assert.AreEqual(10, rates.Breakdown.Shipping.GSTTaxableAmount);
			Assert.AreEqual(0.05, rates.Breakdown.Shipping.GSTTaxRate);
			Assert.AreEqual(0.5, rates.Breakdown.Shipping.GST);
			Assert.AreEqual(10, rates.Breakdown.Shipping.PSTTaxableAmount);
			Assert.AreEqual(0.08, rates.Breakdown.Shipping.PSTTaxRate);
			Assert.AreEqual(0.8, rates.Breakdown.Shipping.PST);
			Assert.AreEqual(0, rates.Breakdown.Shipping.QSTTaxableAmount);
			Assert.AreEqual(0, rates.Breakdown.Shipping.QSTTaxRate);
			Assert.AreEqual(0, rates.Breakdown.Shipping.QST);

			// Line Items
			Assert.AreEqual(16.95, rates.Breakdown.LineItems[0].TaxableAmount);
			Assert.AreEqual(2.2, rates.Breakdown.LineItems[0].TaxCollectable);
			Assert.AreEqual(0.13, rates.Breakdown.LineItems[0].CombinedTaxRate);
			Assert.AreEqual(16.95, rates.Breakdown.LineItems[0].GSTTaxableAmount);
			Assert.AreEqual(0.05, rates.Breakdown.LineItems[0].GSTTaxRate);
			Assert.AreEqual(0.85, rates.Breakdown.LineItems[0].GST);
			Assert.AreEqual(16.95, rates.Breakdown.LineItems[0].PSTTaxableAmount);
			Assert.AreEqual(0.08, rates.Breakdown.LineItems[0].PSTTaxRate);
			Assert.AreEqual(1.36, rates.Breakdown.LineItems[0].PST);
			Assert.AreEqual(0, rates.Breakdown.LineItems[0].QSTTaxableAmount);
			Assert.AreEqual(0, rates.Breakdown.LineItems[0].QSTTaxRate);
			Assert.AreEqual(0, rates.Breakdown.LineItems[0].QST);
		}
	}
}