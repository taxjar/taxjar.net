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
	}
}