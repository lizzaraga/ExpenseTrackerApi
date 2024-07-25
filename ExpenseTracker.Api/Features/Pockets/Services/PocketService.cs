using System.Transactions;
using ExpenseTracker.Api.Features.Pockets.Dtos;
using ExpenseTracker.Api.Features.Pockets.Interfaces;
using ExpenseTracker.Api.Features.Transfers.Interfaces;
using ExpenseTracker.Database;
using ExpenseTracker.Database.Entities;

namespace ExpenseTracker.Api.Features.Pockets.Services;

public class PocketService(
    ILogger<PocketService> logger,
    ITransferService transferService,
    ExpenseTrackerDbContext dbContext
    ): IPocketService
{
    public async Task<Pocket> CreatePocket(CreatePocketDto dto, Purse parent)
    {
        var pocket = new Pocket()
        {
            Name = dto.Name,
            Purse = parent
        };
        using (var scope = new TransactionScope())
        {
            try
            {
                dbContext.Pockets.Add(pocket);
                dbContext.SaveChanges();
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

        if (dto.InitialBalance > 0)
        {
            var result = await transferService.MakePurseToPocketTransfer(pocket, parent, dto.InitialBalance);
            pocket = result.Item2;
        }

        return pocket;
    }

    public Task<Pocket> MakeExpense(Pocket pocket, double amount)
    {
        using (var scope = new TransactionScope())
        {
            try
            {
                pocket.Balance -= amount;
                dbContext.Update(pocket);
                var pocketExpense = new PocketExpenseHistory()
                {
                    NextPocketBalance = pocket.Balance,
                    Amount = amount,
                    Pocket = pocket
                };
                dbContext.PocketExpenseHistories.Add(pocketExpense);
                dbContext.SaveChanges();
                return Task.FromResult(pocket);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            finally{ scope.Complete(); }
        }
    }
}