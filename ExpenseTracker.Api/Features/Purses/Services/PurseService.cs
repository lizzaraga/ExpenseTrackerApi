using System.Transactions;
using AutoMapper;
using ExpenseTracker.Api.Features.Purses.Dtos;
using ExpenseTracker.Api.Features.Purses.Interfaces;
using ExpenseTracker.Database;
using ExpenseTracker.Database.Entities;

namespace ExpenseTracker.Api.Features.Purses.Services;

public class PurseService(
    ExpenseTrackerDbContext dbContext,
    IMapper mapper,
    IPurseIncomeService purseIncomeService
    ): IPurseService
{
    public async Task<Purse> CreatePurse(CreatePurseDto dto, UserAccount owner)
    {
        var purse = mapper.Map<Purse>(dto);
        purse.UserAccount = owner;
        using (var scope = new TransactionScope())
        {
            try
            {
                dbContext.Purses.Add(purse);
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
            purse = await purseIncomeService.IncreaseIncome(purse, dto.InitialBalance);
        }
        return purse;
    }

    public Task<IEnumerable<Purse>> GetUserPurses(string ownerId)
    {
        var result = dbContext.Purses.Where(p => p.UserAccountId.Equals(ownerId)).ToList();
        return Task.FromResult(result.AsEnumerable());
    }
}