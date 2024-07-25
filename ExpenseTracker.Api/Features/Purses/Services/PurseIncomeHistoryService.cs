using System.Transactions;
using ExpenseTracker.Api.Features.Purses.Interfaces;
using ExpenseTracker.Database;
using ExpenseTracker.Database.Entities;
using TransactionScope = System.Transactions.TransactionScope;

namespace ExpenseTracker.Api.Features.Purses.Services;

public class PurseIncomeHistoryService(
    ExpenseTrackerDbContext dbContext
    ): IPurseIncomeService
{
    public Task<Purse> IncreaseIncome(Purse purse, double amount)
    {
        using (var scope = new TransactionScope())
        {
            try
            {
                
                purse.Balance += amount;
                var purseIncome = new PurseIncomeHistory()
                {
                    NextPurseBalance = purse.Balance,
                    Amount = amount,
                    Purse = purse
                };
                dbContext.PurseIncomeHistories.Add(purseIncome);
                dbContext.SaveChanges();
                return Task.FromResult(purse);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            finally
            {
                scope.Complete();
            }
        }
        
    }
}