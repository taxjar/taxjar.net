using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Taxjar;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddTaxJar(
        this IServiceCollection services,
        Action<TaxjarApiOptions>? setupAction = default)
    {
        if (services is null)
        {
            throw new ArgumentNullException(
                nameof(services), "A service collection is required.");
        }

        services.AddOptions<TaxjarApiOptions>()
            .Configure<IConfiguration>(
                (settings, configuration) =>
                    configuration.GetSection(TaxjarApiOptions.SectionName).Bind(settings));

        services.TryAddSingleton<ITaxjarApi, TaxjarApi>();

        if (setupAction != default)
        {
            services.PostConfigure(setupAction);
        }

        return services;
    }
}