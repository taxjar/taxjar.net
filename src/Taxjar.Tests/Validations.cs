using Newtonsoft.Json;
using NUnit.Framework;
using System.Threading.Tasks;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;

namespace Taxjar.Tests
{
	[TestFixture]
	public class ValidationTests
	{
		[Test]
		public void when_validating_a_vat_number()
		{
            var body = JsonConvert.DeserializeObject<ValidationResponse>(TaxjarFixture.GetJSON("validation.json"));

            Bootstrap.server.Given(
                Request.Create()
                    .WithPath("/v2/validation")
                    .UsingGet()
            ).RespondWith(
                Response.Create()
                    .WithStatusCode(200)
                    .WithHeader("Content-Type", "application/json")
                    .WithBodyAsJson(body)
            );

            var validation = Bootstrap.client.Validate(new {
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
			Assert.AreEqual("11 RUE AMPEREn26600 PONT DE L ISERE", validation.ViesResponse.Address);
		}

        [Test]
        public async Task when_validating_a_vat_number_async()
        {
            var body = JsonConvert.DeserializeObject<ValidationResponse>(TaxjarFixture.GetJSON("validation.json"));

            Bootstrap.server.Given(
                Request.Create()
                    .WithPath("/v2/validation")
                    .UsingGet()
            ).RespondWith(
                Response.Create()
                    .WithStatusCode(200)
                    .WithHeader("Content-Type", "application/json")
                    .WithBodyAsJson(body)
            );

            var validationRequest = await Bootstrap.client.ValidateAsync(new
            {
                vat = "FR40303265045"
            });

            var validation = validationRequest.Validation;

            Assert.AreEqual(true, validation.Valid);
            Assert.AreEqual(true, validation.Exists);
            Assert.AreEqual(true, validation.ViesAvailable);
            Assert.AreEqual("FR", validation.ViesResponse.CountryCode);
            Assert.AreEqual("40303265045", validation.ViesResponse.VatNumber);
            Assert.AreEqual("2016-02-10", validation.ViesResponse.RequestDate);
            Assert.AreEqual(true, validation.ViesResponse.Valid);
            Assert.AreEqual("SA SODIMAS", validation.ViesResponse.Name);
            Assert.AreEqual("11 RUE AMPEREn26600 PONT DE L ISERE", validation.ViesResponse.Address);
        }
    }
}