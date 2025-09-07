using DXApplication2.Domain.Data;
using DXApplication2.Domain.Services;
using DXApplication2.Infrastructure.Repositories;
using LiningCheckRecord;
using Microsoft.EntityFrameworkCore;

namespace DXApplication2.Infrastructure.Data;

public class SQLiteUnitOfWork : IDisposable {
   public readonly LiningCheckContext context;
    readonly ICacheService cacheService;

    GenericRepository<DHFOrder>? customersRepository;
    GenericRepository<LiningSpool>? spoolRepository;
    GenericRepository<ExcelSheet>? sheetRepository;
    GenericRepository<CheckerTable>? peoplesRepository;
    public GenericRepository<DHFOrder> CustomersRepository {
        get => customersRepository ??= new GenericRepository<DHFOrder>(context, cacheService);
    } 
    public GenericRepository<LiningSpool> SpoolRepository {
        get => spoolRepository ??= new GenericRepository<LiningSpool>(context, cacheService);
    } 
    public GenericRepository<ExcelSheet> SheetRepository {
        get => sheetRepository ??= new GenericRepository<ExcelSheet>(context, cacheService);
    }  public GenericRepository<CheckerTable> PeoplesRepository {
        get => peoplesRepository ??= new GenericRepository<CheckerTable>(context, cacheService);
    }

    public SQLiteUnitOfWork(ICacheService cacheService) {
        this.cacheService = cacheService;
      //  this.context = new EntitiesContext();
        this.context = new LiningCheckContext();
    }

    public Task SaveAsync() {
        return Task.Run(async () => {
            try {
               await  context.SaveChangesAsync();
				PeoplesRepository.ExecuteCacheUpdateActions();
				SheetRepository.ExecuteCacheUpdateActions();
              var sheets=  SheetRepository.GetAsync();
              var spools=  SpoolRepository.GetAsync();
              //  SpoolRepository.ExecuteCacheUpdateActions();
            }
			catch (DbUpdateException ex) { 
				CustomersRepository.ClearCacheUpdateActions();
				SheetRepository.ClearCacheUpdateActions();
				SpoolRepository.ClearCacheUpdateActions();
                PeoplesRepository.ClearCacheUpdateActions();
				throw;
            }
        });
    }
    public void Dispose() {
        context.Dispose();
    }
}