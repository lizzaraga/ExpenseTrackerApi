using ExpenseTracker.Api.Features.Authentication.Dtos;
using ExpenseTracker.Database.Entities;

namespace ExpenseTracker.Api.Features.Authentication.Interfaces;

public interface IAuthService
{
    Task<UserToken> Login(UserAccount userAccount);
    Task<bool> Register(String email, string password);
}