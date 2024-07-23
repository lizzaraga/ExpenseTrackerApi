using ExpenseTracker.Database.Entities;

namespace ExpenseTracker.Api.Features.Purses.Interfaces;

public interface IPurseIncomeService
{
    Task<Purse> IncreaseIncome(Purse purse, double amount);
}