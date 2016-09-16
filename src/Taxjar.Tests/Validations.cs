using System;
using System.Net;
using System.IO;
using Taxjar;
using HttpMock;
using NUnit.Framework;

namespace Taxjar.Tests
{
	[TestFixture]
	public class ValidationTests
	{
		internal TaxjarApi client;

		[SetUp]
		public void Init()
		{
			this.client = new TaxjarApi("foo123", new { apiUrl = "http://localhost:9191/v2/" });
		}

		[Test]
		public void when_validating_a_vat_number()
		{
			var stubHttp = HttpMockRepository.At("http://localhost:9191");

			stubHttp.Stub(x => x.Get("/v2/validation"))
					.Return(TaxjarFixture.GetJSON("validation.json"))
					.OK();

			var validation = client.Validate(new {
				vat = "FR40303265045"
			});

			Assert.AreEqual(true, validation.Valid);
			Assert.AreEqual(true, validation.Exists);
			Assert.AreEqual(true, validation.ViesAvailable);
			Assert.AreEqual("FR", validation.ViesResponse.CountryCode);
			Assert.AreEqual("40303265045", validation.ViesResponse.VatNumber);
			Assert.AreEqual("2016-02-10", validation.ViesResponse.RequestDate);
			Assert.AreEqual(true, validation.ViesResponse.Valid);
			Assert.AreEqual("SA SODIMAS", validation.ViesResponse.Name);
			Assert.AreEqual("11 RUE AMPERE\n26600 PONT DE L ISERE", validation.ViesResponse.Address);
		}
	}
}