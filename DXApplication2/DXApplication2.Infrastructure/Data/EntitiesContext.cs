using DXApplication2.Domain.Data;
using LiningCheckRecord;
using Microsoft.EntityFrameworkCore;

namespace DXApplication2.Infrastructure.Data;

public class EntitiesContext : DbContext {
    public DbSet<Customer>? Customers { get; set; }
	public DbSet<DHFOrder1>? DHFOrders { get; set; }
	protected override void OnModelCreating(ModelBuilder modelBuilder) {
        modelBuilder.Entity<Customer>()
            .HasIndex(c => c.Id)
            .IsUnique(); 
        
        //modelBuilder.Entity<DHFOrder1>()
        //    .HasIndex(c => c.OrderNo)
        //    .IsUnique();
        
        Seed(modelBuilder);
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
        string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "data.db");
        optionsBuilder.UseSqlite($"Filename={dbPath}");
        base.OnConfiguring(optionsBuilder);
    }

    private void Seed(ModelBuilder modelBuilder) {
        var customers = new List<Customer>();
        for (int i = 1; i <= 10; i++)
            customers.Add(new Customer(i));
        modelBuilder.Entity<Customer>().HasData(customers);

		var customers1 = new List<DHFOrder1>();

		customers1.Add(new DHFOrder1 { Id = 1, Custormer = $"JFE", SiteName = "JFE1", OrderNo = $"00001-000", Total = 10 });
		modelBuilder.Entity<DHFOrder1>().HasData(customers1);
	}
}