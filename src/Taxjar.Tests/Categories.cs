using Newtonsoft.Json;
using NUnit.Framework;
using System.Threading.Tasks;
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
                    .WithHeader("Content-Type", "application/json")
                    .WithBodyAsJson(body)
            );

            var categories = Bootstrap.client.Categories();

            Assert.AreEqual(17, categories.Count);
            Assert.AreEqual("Clothing", categories[0].Name);
            Assert.AreEqual("20010", categories[0].ProductTaxCode);
            Assert.AreEqual("All human wearing apparel suitable for general use", categories[0].Description);
		}

        [Test]
        public async Task when_listing_tax_categories_async()
        {
            var body = JsonConvert.DeserializeObject<CategoriesResponse>(TaxjarFixture.GetJSON("categories.json"));

            Bootstrap.server.Given(
                Request.Create()
                    .WithPath("/v2/categories")
                    .UsingGet()
            ).RespondWith(
                Response.Create()
                    .WithStatusCode(200)
                    .WithHeader("Content-Type", "application/json")
                    .WithBodyAsJson(body)
            );

            var categories = await Bootstrap.client.CategoriesAsync();

            Assert.AreEqual(17, categories.Count);
            Assert.AreEqual("Clothing", categories[0].Name);
            Assert.AreEqual("20010", categories[0].ProductTaxCode);
            Assert.AreEqual("All human wearing apparel suitable for general use", categories[0].Description);
        }
    }
}