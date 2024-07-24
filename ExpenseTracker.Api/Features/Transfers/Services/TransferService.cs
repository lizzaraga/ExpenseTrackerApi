using System.Transactions;
using ExpenseTracker.Api.Features.Transfers.Interfaces;
using ExpenseTracker.Database;
using ExpenseTracker.Database.Entities;

namespace ExpenseTracker.Api.Features.Transfers.Services;

public class TransferService(
    ILogger<TransferService> logger,
    ExpenseTrackerDbContext dbContext
    ): ITransferService
{
    public async Task<(Purse, Pocket, PursePocketTransfer)> MakePurseToPocketTransfer(Pocket pocket, Purse purse, double amount)
    {
        using (var scope = new TransactionScope())
        {
            try
            {
                pocket.Balance += amount;
                purse.Balance -= amount;
                dbContext.Update(pocket);
                dbContext.Update(purse);
                var pursePocketTransfer = new PursePocketTransfer()
                {
                    Amount = amount,
                    Mode = PursePocketTransferMode.PurseToPocket,
                    Pocket = pocket,
                    Purse = purse
                };
                dbContext.PursePocketTransfers.Add(pursePocketTransfer);
                await dbContext.SaveChangesAsync();
                scope.Complete();
                return (purse, pocket, pursePocketTransfer);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }

    public async Task<(Purse, Pocket, PursePocketTransfer)> MakePocketToPurseTransfer(Pocket pocket, Purse purse, double amount)
    {
        using (var scope = new TransactionScope())
        {
            try
            {
                pocket.Balance -= amount;
                purse.Balance += amount;
                dbContext.Update(pocket);
                dbContext.Update(purse);
                var pursePocketTransfer = new PursePocketTransfer()
                {
                    Amount = amount,
                    Mode = PursePocketTransferMode.PocketToPurse,
                    Pocket = pocket,
                    Purse = purse
                };
                dbContext.PursePocketTransfers.Add(pursePocketTransfer);
                await dbContext.SaveChangesAsync();
                scope.Complete();
                return (purse, pocket, pursePocketTransfer);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}