using ExpenseTracker.Database.Entities;

namespace ExpenseTracker.Api.Features.Transfers.Interfaces;

public interface ITransferService
{
    Task<(Purse, Pocket, PursePocketTransfer)> MakePurseToPocketTransfer(Pocket pocket, Purse purse, double amount);
    Task<(Purse, Pocket, PursePocketTransfer)> MakePocketToPurseTransfer(Pocket pocket, Purse purse, double amount);
    Task<(Pocket From, Pocket To, PocketPocketTransfer)> MakePocketToPocketTransfer(Purse purse, Pocket from, Pocket to, double amount);
}