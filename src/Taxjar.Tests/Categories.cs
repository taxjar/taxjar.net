using Newtonsoft.Json;
using NUnit.Framework;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;

namespace Taxjar.Tests
{
	[TestFixture]
    public class CategoriesTests
	{
		[Test]
		public void when_listing_tax_categories()
		{
            var body = JsonConvert.DeserializeObject<CategoriesResponse>(TaxjarFixture.GetJSON("categories.json"));

            Bootstrap.server.Given(
                Request.Create()
                    .WithPath("/v2/categories")
                    .UsingGet()
            ).RespondWith(
                Response.Create()
                    .WithStatusCode(200)
                    .WithBodyAsJson(body)
            );

            var categories = Bootstrap.client.Categories();

            Assert.AreEqual(17, categories.Count);
            Assert.AreEqual("Clothing", categories[0].Name);
            Assert.AreEqual("20010", categories[0].ProductTaxCode);
            Assert.AreEqual("All human wearing apparel suitable for general use", categories[0].Description);
		}
	}
}