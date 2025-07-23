using System.Collections;
using DXApplication2.Domain.Services;
using Microsoft.Extensions.Caching.Memory;

namespace DXApplication2.Infrastructure.Services;

public class MemoryCacheService : ICacheService, IDisposable {
    readonly MemoryCache Cache = new MemoryCache(new MemoryCacheOptions());
    readonly TimeSpan CacheDuration = TimeSpan.FromHours(1);

    public void ClearCollectionCache(string collectionKey) {
        Cache.Remove(collectionKey);
    }
    public TItem? GetOrCreate<TItem>(string cacheKey, Func<ICacheEntry, TItem> factory) {
        return Cache.GetOrCreate(cacheKey, entry => {
            entry.SetAbsoluteExpiration(CacheDuration);
            return factory.Invoke(entry);
        });
    }
    public Task<TItem?> GetOrCreateAsync<TItem>(string cacheKey, Func<ICacheEntry, Task<TItem>> factory) {
        return Cache.GetOrCreateAsync(cacheKey, async entry => {
            entry.SetAbsoluteExpiration(CacheDuration);
            return await factory.Invoke(entry);
        });
    }
    public void UpdateCollectionCache(string collectionKey, Action<IList> updateAction) {
        Cache.TryGetValue(collectionKey, out var cachedCollection);
        if (cachedCollection is IList listCollection)
            updateAction(listCollection);
    }
    public void Dispose() {
        Cache.Dispose();
    }
}