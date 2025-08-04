using System.Collections;
using DXApplication2.Domain.Repositories;
using DXApplication2.Domain.Services;
using DXApplication2.Infrastructure.Data;
using LiningCheckRecord;
using Microsoft.EntityFrameworkCore;

namespace DXApplication2.Infrastructure.Repositories;

public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class {
    //const string CollectionCacheKey = nameof(TEntity);
    readonly string CollectionCacheKey;
    readonly DbSet<TEntity> DbSet;
    readonly ICacheService cacheService;
    readonly LiningCheckContext Context;
    readonly List<Action<IList>> PendingCacheUpdateActions;

    public GenericRepository(LiningCheckContext context, ICacheService cacheService) {
        this.cacheService = cacheService;
        Context = context;
        DbSet = context.Set<TEntity>();
        CollectionCacheKey = $"CollectionCache_{typeof(TEntity).FullName}";
        PendingCacheUpdateActions = new List<Action<IList>>();
    }

    public Task<IEnumerable<TEntity>?> GetAsync() {
		//Context.Customers.Load();	

		return Task.Run(() => cacheService.GetOrCreate<IEnumerable<TEntity>>(CollectionCacheKey, _ => DbSet.ToList()));
    }
    public void Add(TEntity item) {
        DbSet.Add(item);
        PendingCacheUpdateActions.Add(cachedList => cachedList.Add(item));
    }
    public void Delete(TEntity item) {
        DbSet.Remove(item);
        PendingCacheUpdateActions.Add(cachedList => cachedList.Remove(item));
    }
    public Task<TEntity?> GetByIdAsync(int id) {
        return Task.Run(() => DbSet.Find(id));
    }
    public void Update(TEntity entityToUpdate) {
        DbSet.Attach(entityToUpdate);
        Context.Entry(entityToUpdate).State = EntityState.Modified;
        PendingCacheUpdateActions.Add(cachedList => {
            int editedItemIndex = ((List<TEntity>)cachedList).IndexOf(entityToUpdate);
            cachedList[editedItemIndex] = entityToUpdate;
        });
    }

    public void ExecuteCacheUpdateActions() {
        foreach (var updateAction in PendingCacheUpdateActions)
            cacheService.UpdateCollectionCache(CollectionCacheKey, cachedList => updateAction(cachedList));
        PendingCacheUpdateActions.Clear();
    }
    public void ClearCacheUpdateActions() {
        PendingCacheUpdateActions.Clear();
    }
}