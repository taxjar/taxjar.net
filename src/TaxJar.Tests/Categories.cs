using System;
using System.Net;
using System.IO;
using Taxjar;
using HttpMock;
using NUnit.Framework;

namespace Taxjar.Tests
{
	[TestFixture]
	public class CategoriesTests
	{
		internal TaxjarApi client;

		[SetUp]
		public void Init()
		{
			this.client = new TaxjarApi("foo123", new { apiUrl = "http://localhost:9191/v2/" });
		}

		[Test]
		public void when_listing_tax_categories()
		{
			var stubHttp = HttpMockRepository.At("http://localhost:9191");

			stubHttp.Stub(x => x.Get("/v2/categories"))
			        .Return(TaxjarFixture.GetJSON("categories.json"))
					.OK();

			var categories = client.Categories();

			Assert.AreEqual(7, categories.Count);
			Assert.AreEqual("Digital Goods", categories[0].Name);
			Assert.AreEqual("31000", categories[0].ProductTaxCode);
			Assert.AreEqual("Digital products transferred electronically, meaning obtained by the purchaser by means other than tangible storage media.", categories[0].Description);
		}
	}
}