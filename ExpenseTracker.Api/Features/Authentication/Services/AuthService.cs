using ExpenseTracker.Api.Features.Authentication.Dtos;
using ExpenseTracker.Api.Features.Authentication.Interfaces;
using ExpenseTracker.Database.Entities;
using Microsoft.AspNetCore.Identity;

namespace ExpenseTracker.Api.Features.Authentication.Services;

public class AuthService(
    UserManager<UserAccount> userManager,
    ILogger<AuthService> logger): IAuthService
{
    public Task<UserToken> Login(UserAccount userAccount)
    {
        throw new NotImplementedException();
    }
}