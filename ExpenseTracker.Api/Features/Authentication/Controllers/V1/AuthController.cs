using ExpenseTracker.Api.Features.Authentication.Dtos;
using ExpenseTracker.Api.Features.Authentication.Interfaces;
using ExpenseTracker.Database.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseTracker.Api.Features.Authentication.Controllers.V1;


[ApiController]
[Route("Api/[controller]")]
public class AuthController(
    ILogger<AuthController> logger,
    IAuthService authService,
    UserManager<UserAccount> userManager): ControllerBase
{

    [HttpPost("Login")]
    public async Task<IActionResult> Login(LoginReqDto request)
    {
        var existingUser = await userManager.FindByEmailAsync(request.Email);
        if (existingUser is null)
        {
            ModelState.AddModelError("Error", "Unauthorized ! Bad credentials");
            return new UnauthorizedObjectResult(ModelState);
        }

        var passwordIsCorrect = await userManager.CheckPasswordAsync(existingUser, request.Password);
        if (!passwordIsCorrect)
        {
            ModelState.AddModelError("Error", "Unauthorized ! Bad credentials");
            return new UnauthorizedObjectResult(ModelState);
        }

        return Ok(await authService.Login(existingUser));
        
    }

    [HttpPost("Register")]
    public async Task<IActionResult> Register(RegisterReqDto request)
    {
        var existingUser = await userManager.FindByEmailAsync(request.Email);
        if (existingUser is not null)
        {
            ModelState.AddModelError("Error", "This email is already used !");
            return BadRequest(new ValidationProblemDetails(ModelState));
        }

        var result = await authService.Register(email: request.Email, request.Password);
        if (result) return NoContent();
        ModelState.AddModelError("Error", "Unexpected error !!! Unable to register a new user");
        return BadRequest(new ValidationProblemDetails(ModelState));
    }

    [Authorize]
    [HttpGet("Users")]
    public IActionResult GetUsers()
    {
        return Ok(new string[]{"User 1", "User 2"});
    }
}