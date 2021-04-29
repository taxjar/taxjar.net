using System.IO;
using Newtonsoft.Json;
using NUnit.Framework;
using WireMock.Server;
using WireMock.Settings;

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
            if (server == null)
            {
                server = FluentMockServer.Start(new FluentMockServerSettings
                {
                    Urls = new[] { "http://localhost:9191" }
                });
            }

            var options = GetTestOptions();
            apiKey = options.ApiToken;
            client = new TaxjarApi(apiKey, new { apiUrl = "http://localhost:9191" });
        }

        [OneTimeTearDown]
        public static void Destroy()
        {
            server.Stop();
        }

        private static TestOptions GetTestOptions()
        {
            if (File.Exists("../../../secrets.json"))
            {
                var json = File.ReadAllText("../../../secrets.json");
                var options = JsonConvert.DeserializeObject<TestOptions>(json);
                return options;
            }

            return new TestOptions();
        }

        private class TestOptions
        {
            public string ApiToken { get; set; } = "69c23367-5696-473d-a3e2-2142564cacae";
        }
    }
}
