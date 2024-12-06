using Microsoft.AspNetCore.OutputCaching;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace HybridOutputCaching
{
    public class HybridOutputCacheStore : IOutputCacheStore
    {
        private readonly HybridCache _cache;
        private readonly IDistributedCache _distributedCache;
        private readonly object _tagsLock = new();
        private readonly ILogger _logger;

        public HybridOutputCacheStore(HybridCache cache, IDistributedCache distributedCache, ILoggerFactory loggerFactory)
        {

            ArgumentNullException.ThrowIfNull(cache);
            _cache = cache;
            _distributedCache = distributedCache;
            _logger = loggerFactory.CreateLogger<HybridOutputCacheStore>();
        }

        public ValueTask EvictByTagAsync(string tag, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(tag);

            lock (_tagsLock)
            {
                _logger.LogInformation("Evicting By Tag");
                _cache.RemoveByTagAsync(tag, cancellationToken);
            }

            return ValueTask.CompletedTask;
        }

        public async ValueTask<byte[]?> GetAsync(string key, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(key);

            var entry = await _distributedCache.GetAsync(key, cancellationToken);

            _logger.LogInformation("Getting value by key: {key}", key);
            return entry;
        }

        public ValueTask SetAsync(string key, byte[] value, string[]? tags, TimeSpan validFor, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(key);
            ArgumentNullException.ThrowIfNull(value);

            if (tags != null)
            {
                // Lock with SetEntry() to prevent EvictByTagAsync() from trying to remove a tag whose entry hasn't been added yet.
                // It might be acceptable to not lock SetEntry() since in this case Remove(key) would just no-op and the user retry to evict.

                lock (_tagsLock)
                {

                    _logger.LogInformation("Setting value with key: {key} and tags: {tags}", key, tags);
                    SetEntry(key, value, tags, validFor);
                }
            }
            else
            {
                _logger.LogInformation("Setting value with key: {key} No tags", key);
                SetEntry(key, value, tags, validFor);
            }

            return ValueTask.CompletedTask;
        }

        void SetEntry(string key, byte[] value, string[]? tags, TimeSpan validFor)
        {
            Debug.Assert(key != null);

            var hoptions = new HybridCacheEntryOptions() { Expiration = validFor };


            var options = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = validFor,
                Size = value.Length
            };

            if (tags != null && tags.Length > 0)
            {
                // Remove cache keys from tag lists when the entry is evicted
                //    options.RegisterPostEvictionCallback(RemoveFromTags, tags);
            }

            _cache.SetAsync(key, value, hoptions, tags, default);
        }
    }
}

