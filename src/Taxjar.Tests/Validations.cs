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
        [SetUp]
        public static void Init()
        {
            Bootstrap.client = new TaxjarApi(Bootstrap.apiKey, new { apiUrl = "http://localhost:9191" });
            Bootstrap.server.ResetMappings();
        }

        [Test]
        public void when_validating_an_address()
        {
            var body = JsonConvert.DeserializeObject<AddressValidationResponse>(TaxjarFixture.GetJSON("addresses.json"));

            Bootstrap.server.Given(
                Request.Create()
                    .WithPath("/v2/addresses/validate")
                    .UsingPost()
            ).RespondWith(
                Response.Create()
                    .WithStatusCode(200)
                    .WithBodyAsJson(body)
            );

            var addresses = Bootstrap.client.ValidateAddress(new
            {
                country = "US",
                state = "AZ",
                zip = "85297",
                city = "Gilbert",
                street = "3301 South Greenfield Rd"
            });

            Assert.AreEqual("85297-2176", addresses[0].Zip);
            Assert.AreEqual("3301 S Greenfield Rd", addresses[0].Street);
            Assert.AreEqual("AZ", addresses[0].State);
            Assert.AreEqual("US", addresses[0].Country);
            Assert.AreEqual("Gilbert", addresses[0].City);
        }

        [Test]
        public void when_validating_an_address_with_multiple_matches()
        {
            var body = JsonConvert.DeserializeObject<AddressValidationResponse>(TaxjarFixture.GetJSON("addresses_multiple.json"));

            Bootstrap.server.Given(
                Request.Create()
                    .WithPath("/v2/addresses/validate")
                    .UsingPost()
            ).RespondWith(
                Response.Create()
                    .WithStatusCode(200)
                    .WithBodyAsJson(body)
            );

            var addresses = Bootstrap.client.ValidateAddress(new
            {
                state = "AZ",
                city = "Phoenix",
                street = "1109 9th"
            });

            Assert.AreEqual("85007-3646", addresses[0].Zip);
            Assert.AreEqual("1109 S 9th Ave", addresses[0].Street);
            Assert.AreEqual("AZ", addresses[0].State);
            Assert.AreEqual("US", addresses[0].Country);
            Assert.AreEqual("Phoenix", addresses[0].City);

            Assert.AreEqual("85006-2734", addresses[1].Zip);
            Assert.AreEqual("1109 N 9th St", addresses[1].Street);
            Assert.AreEqual("AZ", addresses[1].State);
            Assert.AreEqual("US", addresses[1].Country);
            Assert.AreEqual("Phoenix", addresses[1].City);
        }

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