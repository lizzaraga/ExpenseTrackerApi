using ExpenseTracker.Database.Interfaces;

namespace ExpenseTracker.Database.Entities;

public class PursePocketTransfer: IDateTrackedEntity
{
    public Guid Id { get; set; }
    public required double Amount { get; set; }
    public required PursePocketTransferMode  Mode { get; set; } 

    public Guid PurseId { get; set; }
    public Guid PocketId { get; set; }
    
    public virtual required Purse Purse { get; set; }
    public virtual required Pocket Pocket { get; set; }
    
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}


public enum PursePocketTransferMode: byte
{
    PurseToPocket = 0x01,
    PocketToPurse =  0x10
}