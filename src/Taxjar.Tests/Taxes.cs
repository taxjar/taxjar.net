using HttpMock;
using NUnit.Framework;

namespace Taxjar.Tests
{
	[TestFixture]
	public class TaxesTests
	{
		internal TaxjarApi client;

		[SetUp]
		public void Init()
		{
			this.client = new TaxjarApi("foo123", new { apiUrl = "http://localhost:9191/v2/" });
		}

		[Test]
		public void when_calculating_sales_tax_for_an_order()
		{
			var stubHttp = HttpMockRepository.At("http://localhost:9191");

			stubHttp.Stub(x => x.Post("/v2/taxes"))
					.Return(TaxjarFixture.GetJSON("taxes.json"))
					.OK();

			var rates = client.TaxForOrder(new {
				from_country =  "US",
				from_zip = "07001",
				from_state = "NJ",
				to_country = "US",
				to_zip = "07446",
				to_state = "NJ",
				amount = 16.50,
				shipping = 1.50
			});

			Assert.AreEqual(16.5, rates.OrderTotalAmount);
			Assert.AreEqual(1.16, rates.AmountToCollect);
			Assert.AreEqual(true, rates.HasNexus);
			Assert.AreEqual(true, rates.FreightTaxable);
			Assert.AreEqual("destination", rates.TaxSource);

			// Breakdowns
			Assert.AreEqual(0.11, rates.Breakdown.Shipping.StateAmount);
			Assert.AreEqual(0.07, rates.Breakdown.Shipping.StateSalesTaxRate);
			Assert.AreEqual(0, rates.Breakdown.Shipping.CountyAmount);
			Assert.AreEqual(0, rates.Breakdown.Shipping.CountyTaxRate);
			Assert.AreEqual(0, rates.Breakdown.Shipping.CityAmount);
			Assert.AreEqual(0, rates.Breakdown.Shipping.CityTaxRate);
			Assert.AreEqual(0, rates.Breakdown.Shipping.SpecialDistrictAmount);
			Assert.AreEqual(0, rates.Breakdown.Shipping.SpecialTaxRate);

			Assert.AreEqual(16.5, rates.Breakdown.StateTaxableAmount);
			Assert.AreEqual(1.16, rates.Breakdown.StateTaxCollectable);
			Assert.AreEqual(0, rates.Breakdown.CountyTaxableAmount);
			Assert.AreEqual(0, rates.Breakdown.CountyTaxCollectable);
			Assert.AreEqual(0, rates.Breakdown.CityTaxableAmount);
			Assert.AreEqual(0, rates.Breakdown.CityTaxCollectable);
			Assert.AreEqual(0, rates.Breakdown.SpecialDistrictTaxableAmount);
			Assert.AreEqual(0, rates.Breakdown.SpecialDistrictTaxCollectable);

			// Line Items
			Assert.AreEqual(1, rates.Breakdown.LineItems[0].Id);
			Assert.AreEqual(15, rates.Breakdown.LineItems[0].StateTaxableAmount);
			Assert.AreEqual(0.07, rates.Breakdown.LineItems[0].StateSalesTaxRate);
			Assert.AreEqual(0, rates.Breakdown.LineItems[0].CountyTaxableAmount);
			Assert.AreEqual(0, rates.Breakdown.LineItems[0].CountyTaxRate);
			Assert.AreEqual(0, rates.Breakdown.LineItems[0].CityTaxableAmount);
			Assert.AreEqual(0, rates.Breakdown.LineItems[0].CityTaxRate);
			Assert.AreEqual(0, rates.Breakdown.LineItems[0].SpecialDistrictTaxableAmount);
			Assert.AreEqual(0, rates.Breakdown.LineItems[0].SpecialTaxRate);
		}
	}
}