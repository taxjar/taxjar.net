using System;
using System.Net;
using System.IO;
using Taxjar;
using HttpMock;
using NUnit.Framework;

namespace Taxjar.Tests
{
	[TestFixture]
	public class NexusTests
	{
		internal TaxjarApi client;

		[SetUp]
		public void Init()
		{
			this.client = new TaxjarApi("foo123", new { apiUrl = "http://localhost:9191/v2/" });
		}

		[Test]
		public void when_listing_nexus_regions()
		{
			var stubHttp = HttpMockRepository.At("http://localhost:9191");

			stubHttp.Stub(x => x.Get("/v2/nexus/regions"))
					.Return(TaxjarFixture.GetJSON("nexus_regions.json"))
					.OK();

			var nexusRegions = client.NexusRegions();

			Assert.AreEqual(3, nexusRegions.Count);
			Assert.AreEqual("US", nexusRegions[0].CountryCode);
			Assert.AreEqual("United States", nexusRegions[0].Country);
			Assert.AreEqual("CA", nexusRegions[0].RegionCode);
			Assert.AreEqual("California", nexusRegions[0].Region);
		}
	}
}