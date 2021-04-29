#if NETSTANDARD2_0

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

using System;

using Taxjar;
using Taxjar.Options;

namespace Microsoft.Extensions.Configuration
{
    /// <summary>
    ///  Add TaxjarApi to Dependency Injection.
    /// </summary>
    public static class TaxjarServiceCollectionExtensions
    {
        /// <summary>
        /// Adds <see cref="ITaxjarApi"/> client to DI.
        /// </summary>
        /// <param name="services">The DI services.</param>
        /// <param name="namedOption">Add a named options functionality.</param>
        /// <param name="configure">The configuration for <see cref="TaxjarOptions"/>.</param>
        /// <returns></returns>
        public static IServiceCollection AddTaxjar(this IServiceCollection services,
                                                   string namedOption = "",
                                                   Action<TaxjarOptions, IConfiguration> configure = default)
        {
            var name = string.IsNullOrEmpty(namedOption) ? nameof(TaxjarOptions) : namedOption;

            services.AddOptions<TaxjarOptions>(name)
                    .Configure<IConfiguration>((o, c) =>
                    {
                        c.Bind(name, o);

                        configure?.Invoke(o, c);
                    });

            services.AddTransient<ITaxjarApi, TaxjarApi>(sp =>
            {
                var options = sp.GetRequiredService<IOptionsMonitor<TaxjarOptions>>().Get(name);

                var baseUrl = options.ApiUrl;

                return new TaxjarApi(
                    options.ApiToken,
                    new { apiUrl = baseUrl, timeout = options.Timeout });
            });

            return services;
        }
    }
}
#endif
