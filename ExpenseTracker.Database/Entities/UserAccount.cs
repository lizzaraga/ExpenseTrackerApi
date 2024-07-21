using ExpenseTracker.Database.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace ExpenseTracker.Database.Entities;

public class UserAccount: IdentityUser, IDateTrackedEntity
{
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime? UpdatedAt { get; set; }
}