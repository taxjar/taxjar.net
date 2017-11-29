using System.Net;
using HttpMock;
using NUnit.Framework;

namespace Taxjar.Tests
{
	[TestFixture]
	public class ClientTests
	{
		internal TaxjarApi client;

		[Test]
		public void instantiates_client_with_api_token()
		{
			this.client = new TaxjarApi("foo123");
		}

		[Test]
		public void instantiates_client_with_additional_arguments()
		{
			this.client = new TaxjarApi("foo123", new { apiUrl = "http://localhost:9191/v2/" });
		}

		[Test]
		public void returns_exception_with_invalid_api_token()
		{
			this.client = new TaxjarApi("foo123", new { apiUrl = "http://localhost:9191/v2/" });

			var stubHttp = HttpMockRepository.At("http://localhost:9191");

			stubHttp.Stub(x => x.Get("/v2/categories"))
					.Return("{\"error\":\"Unauthorized\",\"detail\":\"Not authorized for route 'GET /v2/categories'\",\"status\":401}")
					.WithStatus(HttpStatusCode.Unauthorized);

			var taxjarException = Assert.Throws<TaxjarException>(() => client.Categories());

			Assert.AreEqual(HttpStatusCode.Unauthorized, taxjarException.HttpStatusCode);
			Assert.AreEqual("Unauthorized", taxjarException.TaxjarError.Error);
			Assert.AreEqual("Not authorized for route 'GET /v2/categories'", taxjarException.TaxjarError.Detail);
			Assert.AreEqual("401", taxjarException.TaxjarError.StatusCode);
		}
	}
}