using NUnit.Framework;
using WireMock.Server;
using WireMock.Settings;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;

namespace Taxjar.Tests
{
    [SetUpFixture]
    public class Bootstrap
    {
        public static TaxjarApi client;
        public static FluentMockServer server;
        public static string apiKey;

        [OneTimeSetUp]
        public static void Init()
        {
            DotNetEnv.Env.Load("../../../.env");

            if (server == null)
            {
                server = FluentMockServer.Start(new FluentMockServerSettings
                {
                    Urls = new[] { "http://localhost:9191" }
                });
            }

            apiKey = System.Environment.GetEnvironmentVariable("TAXJAR_API_KEY") ?? "foo123";
            client = new TaxjarApi(apiKey, new { apiUrl = "http://localhost:9191" });
        }

        [OneTimeTearDown]
        public static void Destroy()
        {
            server.Stop();
        }
    }
}
