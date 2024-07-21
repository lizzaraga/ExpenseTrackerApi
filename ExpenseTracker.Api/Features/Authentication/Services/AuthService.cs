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

    public async Task<bool> Register(string email, string password)
    {
        var userAccount = new UserAccount()
        {
            Email = email,
            UserName = email,
            EmailConfirmed = true
        };
        var result = await userManager.CreateAsync(userAccount, password);
        if (result == IdentityResult.Success) return true;
        return false;
    }
}