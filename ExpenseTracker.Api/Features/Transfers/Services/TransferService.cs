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
    public Task<(Purse, Pocket, PursePocketTransfer)> MakePurseToPocketTransfer(Pocket pocket, Purse purse, double amount)
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
                    NextPocketBalance = pocket.Balance,
                    NextPurseBalance = purse.Balance,
                    Mode = PursePocketTransferMode.PurseToPocket,
                    Pocket = pocket,
                    Purse = purse
                };
                dbContext.PursePocketTransfers.Add(pursePocketTransfer);
                dbContext.SaveChanges();
                return Task.FromResult((purse, pocket, pursePocketTransfer));
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

    public Task<(Purse, Pocket, PursePocketTransfer)> MakePocketToPurseTransfer(Pocket pocket, Purse purse, double amount)
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
                    NextPocketBalance = pocket.Balance,
                    NextPurseBalance = purse.Balance,
                    Amount = amount,
                    Mode = PursePocketTransferMode.PocketToPurse,
                    Pocket = pocket,
                    Purse = purse
                };
                dbContext.PursePocketTransfers.Add(pursePocketTransfer);
                dbContext.SaveChanges();
                return Task.FromResult((purse, pocket, pursePocketTransfer));
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

    public Task<(Pocket From, Pocket To, PocketPocketTransfer)> MakePocketToPocketTransfer(Purse purse, Pocket from, Pocket to, double amount)
    {
        using (var scope = new TransactionScope())
        {

            try
            {
                from.Balance -= amount;
                to.Balance += amount;
                dbContext.Update(from);
                dbContext.Update(to);
                var pocketPocketTransfer = new PocketPocketTransfer()
                {
                    FromPocketNextBalance = from.Balance,
                    ToPocketNextBalance = to.Balance,
                    Amount = amount,
                    From = from,
                    To = to,
                    Purse = purse
                };
                dbContext.PocketPocketTransfers.Add(pocketPocketTransfer);
                dbContext.SaveChanges();

                return Task.FromResult((from, to, pocketPocketTransfer));
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