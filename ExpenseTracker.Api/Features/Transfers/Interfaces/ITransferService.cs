using ExpenseTracker.Database.Entities;

namespace ExpenseTracker.Api.Features.Transfers.Interfaces;

public interface ITransferService
{
    Task<(Purse, Pocket)> MakePurseToPocketTransfer(Pocket pocket, Purse purse, double amount);
}