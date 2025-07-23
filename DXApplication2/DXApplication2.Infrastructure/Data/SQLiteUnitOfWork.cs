using DXApplication2.Domain.Data;
using DXApplication2.Domain.Services;
using DXApplication2.Infrastructure.Repositories;
using LiningCheckRecord;

namespace DXApplication2.Infrastructure.Data;

public class SQLiteUnitOfWork : IDisposable {
    readonly LiningCheckContext context;
    readonly ICacheService cacheService;

    GenericRepository<DHFOrder>? customersRepository;
    GenericRepository<LiningSpool>? spoolRepository;
    GenericRepository<ExcelSheet>? sheetRepository;
    public GenericRepository<DHFOrder> CustomersRepository {
        get => customersRepository ??= new GenericRepository<DHFOrder>(context, cacheService);
    } 
    public GenericRepository<LiningSpool> SpoolRepository {
        get => spoolRepository ??= new GenericRepository<LiningSpool>(context, cacheService);
    } 
    public GenericRepository<ExcelSheet> SheetRepository {
        get => sheetRepository ??= new GenericRepository<ExcelSheet>(context, cacheService);
    }

    public SQLiteUnitOfWork(ICacheService cacheService) {
        this.cacheService = cacheService;
      //  this.context = new EntitiesContext();
        this.context = new LiningCheckContext();
    }

    public Task SaveAsync() {
        return Task.Run(() => {
            try {
                context.SaveChanges();
                CustomersRepository.ExecuteCacheUpdateActions();
                SheetRepository.ExecuteCacheUpdateActions();
                SpoolRepository.ExecuteCacheUpdateActions();
            } catch {
                CustomersRepository.ClearCacheUpdateActions();
				SheetRepository.ClearCacheUpdateActions();
				SpoolRepository.ClearCacheUpdateActions();
                throw;
            }
        });
    }
    public void Dispose() {
        context.Dispose();
    }
}