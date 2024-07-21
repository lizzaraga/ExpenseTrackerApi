using System.ComponentModel.DataAnnotations.Schema;
using ExpenseTracker.Database.Interfaces;

namespace ExpenseTracker.Database.Entities;

public class Pocket: IDateTrackedEntity
{
    public Guid Id { get; set; }
    public double Balance { get; set; }
    public required string Name { get; set; }
    
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    public Guid PurseId { get; set; }
    public virtual Purse Purse { get; set; } = default!;
}