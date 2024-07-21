using System.ComponentModel.DataAnnotations.Schema;
using ExpenseTracker.Database.Interfaces;

namespace ExpenseTracker.Database.Entities;

public class PocketPocketTransfer: IDateTrackedEntity
{
    public Guid Id { get; set; }

    public required double Amount { get; set; }
    
    [ForeignKey("FromPocketId")]
    public virtual required Pocket From { get; set; }
    [ForeignKey("ToPocketId")]
    public virtual required Pocket To { get; set; }
    [ForeignKey("PurseId")]
    public virtual required Purse Purse { get; set; }

    public Guid FromPocketId { get; set; }
    public Guid ToPocketId { get; set; }
    public Guid PurseId { get; set; }
    
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}