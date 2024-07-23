using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ExpenseTracker.Api.Features.Authentication.Configuration;
using ExpenseTracker.Api.Features.Authentication.Dtos;
using ExpenseTracker.Api.Features.Authentication.Interfaces;
using ExpenseTracker.Database.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace ExpenseTracker.Api.Features.Authentication.Services;

public class AuthService(
    UserManager<UserAccount> userManager,
    IOptions<JwtConfig> jwtConfigOptions,
    ILogger<AuthService> logger): IAuthService
{
    public Task<UserToken> Login(UserAccount userAccount)
    {
        var expiresAt = DateTime.UtcNow.AddMinutes(10);
        var claims = new[]
        {
            new Claim(ClaimTypes.Email, userAccount.Email!),
            new Claim(ClaimTypes.Sid, userAccount.Id!),
        };
        var securityToken = new JwtSecurityToken(
            issuer: jwtConfigOptions.Value.Issuer,
            audience: jwtConfigOptions.Value.Audience,
            notBefore: DateTime.UtcNow,
            expires: expiresAt,
            claims: claims,
            signingCredentials: new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfigOptions.Value.Secret)),
                SecurityAlgorithms.HmacSha256
                )
            );

        var tokenHandler = new JwtSecurityTokenHandler();

        return Task.FromResult(new UserToken()
        {
            Token = tokenHandler.WriteToken(securityToken),
            ExpiresAt = expiresAt
        });
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