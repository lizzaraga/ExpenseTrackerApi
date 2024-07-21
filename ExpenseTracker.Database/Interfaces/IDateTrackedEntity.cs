namespace ExpenseTracker.Database.Interfaces;

public interface IDateTrackedEntity
{
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}