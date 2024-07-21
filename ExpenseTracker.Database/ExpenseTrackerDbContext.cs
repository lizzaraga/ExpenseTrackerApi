using ExpenseTracker.Database.Entities;
using ExpenseTracker.Database.Interfaces;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Database;

public class ExpenseTrackerDbContext(DbContextOptions<ExpenseTrackerDbContext> options): IdentityDbContext<UserAccount>(options)
{
    public DbSet<Purse> Purses { get; set; }
    public DbSet<Pocket> Pockets { get; set; }
    public DbSet<PurseIncomeHistory> PurseIncomeHistories { get; set; }
    public DbSet<PocketExpenseHistory> PocketExpenseHistories { get; set; }
    public DbSet<PursePocketTransfer> PursePocketTransfers { get; set; }
    public DbSet<PocketPocketTransfer> PocketPocketTransfers { get; set; }

    public override int SaveChanges()
    {
        UpdateDateTrackedEntities();
        return base.SaveChanges();
    }

    public override int SaveChanges(bool acceptAllChangesOnSuccess)
    {
        UpdateDateTrackedEntities();
        return base.SaveChanges(acceptAllChangesOnSuccess);
    }

    public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = new CancellationToken())
    {
        UpdateDateTrackedEntities();
        return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        UpdateDateTrackedEntities();
        return base.SaveChangesAsync(cancellationToken);
    }
    

    private void UpdateDateTrackedEntities()
    {
        ChangeTracker.Entries<IDateTrackedEntity>().Where(entry => entry.State == EntityState.Added || entry.State == EntityState.Modified)
            .ToList()
            .ForEach(entry =>
            {
                var now = DateTime.UtcNow;
                if (entry.State == EntityState.Added)
                {
                    IDateTrackedEntity entity = entry.Entity;
                    entity.CreatedAt = now;
                    entity.UpdatedAt = now;
                }
                else
                {
                    IDateTrackedEntity entity = entry.Entity;
                    entity.UpdatedAt = now;
                }
                
            } );
    }
}