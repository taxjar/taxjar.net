#if NETSTANDARD2_0

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

using System;

using Taxjar.Options;

namespace Taxjar.DependencyInjection
{
    /// <summary>
    ///  Add TaxjarApi to Dependency Injection.
    /// </summary>
    public static class TaxjarServiceCollectionExtensions
    {
        public static IServiceCollection AddTaxjar(this IServiceCollection services,
                                                   Action<TaxjarOptions> configure = default)
        {

            services.AddChangeTokenOptions(nameof(TaxjarOptions), string.Empty, configure);

            services.AddTransient<ITaxjarApi, TaxjarApi>(sp =>
            {
                var options = sp.GetRequiredService<IOptions<TaxjarOptions>>().Value;

                var baseUrl = options.IsSandBox ? "https://api.sandbox.taxjar.com" : "https://api.taxjar.com";
                return new TaxjarApi(
                    options.ApiToken,
                    new { apiUrl = baseUrl, timeout = (int)options.Timeout.TotalMilliseconds });
            });

            return services;
        }
    }
}
#endif
