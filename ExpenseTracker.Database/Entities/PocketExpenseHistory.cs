using System.ComponentModel.DataAnnotations.Schema;
using ExpenseTracker.Database.Interfaces;

namespace ExpenseTracker.Database.Entities;

public class PocketExpenseHistory: IDateTrackedEntity
{
    public Guid Id { get; set; }
    public double Amount { get; set; }
    
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    public Guid PocketId { get; set; }
    public virtual Pocket Pocket { get; set; } = default!;
}
