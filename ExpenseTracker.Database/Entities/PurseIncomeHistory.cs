using ExpenseTracker.Database.Interfaces;

namespace ExpenseTracker.Database.Entities;

public class PurseIncomeHistory: IDateTrackedEntity
{
    public Guid Id { get; set; }
    public required double Amount { get; set; }
    
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    public Guid PurseId { get; set; }
    public virtual Purse Purse { get; set; } = default!;
}