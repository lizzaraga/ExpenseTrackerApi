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
                await dbContext.SaveChangesAsync();
                scope.Complete();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        if (dto.InitialBalance > 0)
        {
            var result = await transferService.MakePurseToPocketTransfer(pocket, parent, dto.InitialBalance);
            pocket = result.Item2;
        }

        return pocket;
    }
}