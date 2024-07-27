using ExpenseTracker.Api.Features.Purses.Dtos;
using ExpenseTracker.Database.Entities;

namespace ExpenseTracker.Api.Features.Purses.Interfaces;

public interface IPurseService
{
    Task<Purse> CreatePurse(CreatePurseDto dto, UserAccount owner);
    Task<IEnumerable<Purse>> GetUserPurses(string ownerId);

}