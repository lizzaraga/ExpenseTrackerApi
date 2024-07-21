using System.ComponentModel.DataAnnotations.Schema;

namespace ExpenseTracker.Database.Entities;

public class Purse
{
    public Guid Id { get; set; }
    public required double Balance { get; set; }
    public required string Currency { get; set; }
    public required string Name { get; set; }

    public string UserAccountId { get; set; }
    [ForeignKey("UserAccountId")]
    public virtual UserAccount UserAccount { get; set; } = default!;

}

