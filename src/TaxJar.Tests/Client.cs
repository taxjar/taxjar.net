using System.Collections.Generic;
using System.Net;
using NUnit.Framework;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;

namespace Taxjar.Tests
{
	[TestFixture]
    public class ClientTests
	{
        [SetUp]
        public static void Init()
        {
            Bootstrap.client = new TaxjarApi(Bootstrap.apiKey, new { apiUrl = "http://localhost:9191" });
            Bootstrap.server.ResetMappings();
        }

		[Test, Order(1)]
		public void instantiates_client_with_api_token()
		{
            Bootstrap.client = new TaxjarApi(Bootstrap.apiKey);
		}

		[Test, Order(2)]
		public void instantiates_client_with_additional_arguments()
		{
            Bootstrap.client = new TaxjarApi(Bootstrap.apiKey, new { apiUrl = "http://localhost:9191" });
		}

        [Test, Order(3)]
        public void instantiates_client_with_custom_headers()
        {
            Dictionary<string, string> customHeaders = new Dictionary<string, string>
            {
                { "X-TJ-Expected-Response", "422" }
            };

            Bootstrap.client = new TaxjarApi(Bootstrap.apiKey, new {
                apiUrl = "http://localhost:9191",
                headers = customHeaders
            });

            Assert.AreEqual(Bootstrap.client.GetApiConfig("headers"), customHeaders);
        }

        [Test, Order(4)]
        public void get_api_config()
        {
            Assert.AreEqual(Bootstrap.client.GetApiConfig("apiUrl"), "http://localhost:9191/v2/");
        }

        [Test, Order(5)]
        public void set_api_config()
        {
            Bootstrap.client.SetApiConfig("apiUrl", "https://api.sandbox.taxjar.com");
            Assert.AreEqual(Bootstrap.client.GetApiConfig("apiUrl"), "https://api.sandbox.taxjar.com/v2/");
        }

        [Test, Order(6)]
        public void sets_custom_headers_via_api_config()
        {
            Dictionary<string, string> customHeaders = new Dictionary<string, string>
            {
                { "X-TJ-Expected-Response", "422" }
            };

            Bootstrap.client.SetApiConfig("headers", customHeaders);
            Assert.AreEqual(Bootstrap.client.GetApiConfig("headers"), customHeaders);
        }

		[Test, Order(7)]
		public void returns_exception_with_invalid_api_token()
		{
            Bootstrap.server.Given(
                Request.Create()
                    .WithPath("/v2/categories")
                    .UsingGet()
            ).RespondWith(
                Response.Create()
                    .WithStatusCode(HttpStatusCode.Unauthorized)
                    .WithHeader("Content-Type", "application/json")
                    .WithBodyAsJson(new { error = "Unauthorized", detail = "Not authorized for route 'GET /v2/categories'", status = 401 })
            );

            var taxjarException = Assert.Throws<TaxjarException>(() => Bootstrap.client.Categories());

			Assert.AreEqual(HttpStatusCode.Unauthorized, taxjarException.HttpStatusCode);
			Assert.AreEqual("Unauthorized", taxjarException.TaxjarError.Error);
			Assert.AreEqual("Not authorized for route 'GET /v2/categories'", taxjarException.TaxjarError.Detail);
			Assert.AreEqual("401", taxjarException.TaxjarError.StatusCode);
		}
	}
}