using ExpenseTracker.Database.Entities;
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
    
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
    }
}