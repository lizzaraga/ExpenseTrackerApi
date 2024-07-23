using System.Security.Claims;
using ExpenseTracker.Api.Features.Purses.Dtos;
using ExpenseTracker.Api.Features.Purses.Interfaces;
using ExpenseTracker.Database.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseTracker.Api.Features.Purses.Controllers.V1;

[Authorize]
[ApiController]
[Route("Api/[controller]")]
public class PursesController(
    ILogger<PursesController> logger,
    IPurseService purseService,
    UserManager<UserAccount> userManager): ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<Purse>> CreatePurse([FromBody] CreatePurseDto request)
    {
        Claim idClaim = HttpContext.User.Claims.First(x => x.Type.Equals(ClaimTypes.Sid));
        var userAccount = await userManager.FindByIdAsync(idClaim.Value);
        ArgumentNullException.ThrowIfNull(userAccount);
        var result = await purseService.CreatePurse(request, userAccount);
        return Ok(result);
    }
}