using System.Collections;
using Microsoft.Extensions.Caching.Memory;

namespace DXApplication2.Domain.Services;

public interface ICacheService {
    TItem? GetOrCreate<TItem>(string cacheKey, Func<ICacheEntry, TItem> factory);
    Task<TItem?> GetOrCreateAsync<TItem>(string cacheKey, Func<ICacheEntry, Task<TItem>> factory);
    void UpdateCollectionCache(string collectionKey, Action<IList> updateAction);
    void ClearCollectionCache(string collectionKey);
}