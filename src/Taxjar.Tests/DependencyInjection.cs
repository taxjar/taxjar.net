#if NETCOREAPP2_1

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

using NUnit.Framework;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Taxjar.Options;

namespace Taxjar.Tests
{
    [TestFixture]
    public class DependencyInjection
    {
        [Test]
        public void when_registering_single_client_di()
        {
            var d = new Dictionary<string, string>
            {
                {"TaxjarOptions:ApiToken", "69c23367-5696-473d-a3e2-2142564cacae" },
                {"TaxjarOptions:ApiUrl", TaxjarConstants.SandboxApiUrl },
                {"TaxjarOptions:Timeout", "30" },
            };

            var configBuilder = new ConfigurationBuilder().AddInMemoryCollection(d);

            var services = new ServiceCollection();

            services.AddSingleton<IConfiguration>(configBuilder.Build());
            services.AddTaxjar();

            var sp = services.BuildServiceProvider();

            var options = sp.GetRequiredService<IOptionsMonitor<TaxjarOptions>>().Get(nameof(TaxjarOptions));
            var clinet = sp.GetRequiredService<ITaxjarApi>();

            Assert.AreEqual($"{TaxjarConstants.SandboxApiUrl}/{TaxjarConstants.ApiVersion}/", clinet.apiUrl);
            Assert.AreEqual(TaxjarConstants.SandboxApiUrl, options.ApiUrl);
        }

        [Test]
        public void when_registering_two_clients_di()
        {
            var production = "TaxjarOptionsProduction";
            var sandbox = "TaxjarOptionsSandbox";

            var d = new Dictionary<string, string>
            {
                {$"{sandbox}:ApiToken", "69c23367-5696-473d-a3e2-2142564cacae" },
                {$"{sandbox}:ApiUrl", TaxjarConstants.SandboxApiUrl },
                {$"{sandbox}:Timeout", "40" },

                {$"{production}:ApiToken", "eee4b9f8-1c31-41e2-b21b-cd6b7111596d" },
                {$"{production}:ApiUrl", TaxjarConstants.DefaultApiUrl },
                {$"{production}:Timeout", "30" },
            };

            var configBuilder = new ConfigurationBuilder().AddInMemoryCollection(d);

            var services = new ServiceCollection();

            services.AddSingleton<IConfiguration>(configBuilder.Build());
            services.AddTaxjar(sandbox);
            services.AddTaxjar(production);

            var sp = services.BuildServiceProvider();

            var sandBoxoptions = sp.GetRequiredService<IOptionsMonitor<TaxjarOptions>>().Get(sandbox);
            var clinet = sp.GetServices<ITaxjarApi>();

            Assert.AreEqual(2, clinet.Count());
            Assert.AreEqual(TaxjarConstants.SandboxApiUrl, sandBoxoptions.ApiUrl);
        }
    }
}
#endif
