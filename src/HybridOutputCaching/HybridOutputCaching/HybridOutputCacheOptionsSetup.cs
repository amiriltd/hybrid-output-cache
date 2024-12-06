using Microsoft.AspNetCore.OutputCaching;
using Microsoft.Extensions.Options;
using System;

namespace HybridOutputCaching
{
    internal sealed class HybridOutputCacheOptionsSetup : IConfigureOptions<OutputCacheOptions>
    {
        private readonly IServiceProvider _services;

        public HybridOutputCacheOptionsSetup(IServiceProvider services)
        {
            _services = services;
        }

        public void Configure(OutputCacheOptions options)
        {
           
            // options.ApplicationServices = _services;
        }
    }
}
