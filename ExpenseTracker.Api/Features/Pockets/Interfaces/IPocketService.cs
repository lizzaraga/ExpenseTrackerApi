using ExpenseTracker.Api.Features.Pockets.Dtos;
using ExpenseTracker.Database.Entities;

namespace ExpenseTracker.Api.Features.Pockets.Interfaces;

public interface IPocketService
{
    Task<Pocket> CreatePocket(CreatePocketDto dto, Purse parent);
    Task<Pocket> MakeExpense(Pocket pocket, double amount);
}