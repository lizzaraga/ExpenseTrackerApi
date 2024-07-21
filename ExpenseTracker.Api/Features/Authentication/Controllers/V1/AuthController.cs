using ExpenseTracker.Api.Features.Authentication.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseTracker.Api.Features.Authentication.Controllers.V1;


[ApiController]
[Route("Api/[controller]")]
public class AuthController: ControllerBase
{

    [HttpPost("Login")]
    public IActionResult Login(LoginReqDto request)
    {
        return Ok();
    }

    [HttpPost("Register")]
    public IActionResult Register(RegisterReqDto request)
    {
        return Ok();
    }

    [Authorize]
    [HttpGet("Users")]
    public IActionResult GetUsers()
    {
        return Ok(new string[]{"User 1", "User 2"});
    }
}