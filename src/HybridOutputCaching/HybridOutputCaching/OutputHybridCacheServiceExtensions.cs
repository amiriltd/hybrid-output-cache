using Microsoft.AspNetCore.OutputCaching;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.ObjectPool;
using Microsoft.Extensions.Options;
using System;

namespace HybridOutputCaching
{
    public static class OutputHybridCacheServiceExtensions
    {
        /// <summary>
        /// Add output caching services.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> for adding services.</param>
        /// <returns></returns>
        public static IServiceCollection AddHybridOutputCache(this IServiceCollection services)
        {
            ArgumentNullException.ThrowIfNull(services);

            services.AddTransient<IConfigureOptions<OutputCacheOptions>, HybridOutputCacheOptionsSetup>();

            services.TryAddSingleton<ObjectPoolProvider, DefaultObjectPoolProvider>();

            services.TryAddSingleton<IOutputCacheStore, HybridOutputCacheStore>();
            return services;
        }

        /// <summary>
        /// Add output caching services and configure the related options.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> for adding services.</param>
        /// <param name="configureOptions">A delegate to configure the <see cref="OutputCacheOptions"/>.</param>
        /// <returns></returns>
        public static IServiceCollection AddHybridOutputCache(this IServiceCollection services, Action<OutputCacheOptions> configureOptions)
        {
            ArgumentNullException.ThrowIfNull(services);
            ArgumentNullException.ThrowIfNull(configureOptions);

            services.Configure(configureOptions);
            services.AddHybridOutputCache();

            return services;
        }
    }
}
