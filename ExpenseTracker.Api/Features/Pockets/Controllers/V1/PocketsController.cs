using System.Security.Claims;
using ExpenseTracker.Api.Features.Pockets.Dtos;
using ExpenseTracker.Api.Features.Pockets.Interfaces;
using ExpenseTracker.Database;
using ExpenseTracker.Database.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseTracker.Api.Features.Pockets.Controllers.V1;


[Authorize]
[ApiController]
[Route("Api/[controller]")]
public class PocketsController(
    ILogger<PocketsController> logger,
    IPocketService pocketService,
    ExpenseTrackerDbContext dbContext
    ): ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<Pocket>> CreatePocket(CreatePocketDto request)
    {
        
        var purseParent = dbContext.Purses.FirstOrDefault(p => p.Id.Equals(request.PurseId));
        if (purseParent is null)
        {
            ModelState.AddModelError("error", "Unable to find purse !");
            return BadRequest(ModelState);
        }

        if (request.InitialBalance > purseParent.Balance)
        {
            ModelState.AddModelError("error", "The initial balance is greater than corresponding purse balance !");
            return BadRequest(ModelState);
        }

        var result = await pocketService.CreatePocket(request, purseParent);
        return Ok(result);
    }

    [HttpPost("MakeExpense")]
    public async Task<ActionResult<Pocket>> MakeExpense([FromBody] PocketExpenseDto request)
    {
        var pocket = dbContext.Pockets.FirstOrDefault(p => p.Id.Equals(request.PocketId));
        if (pocket is null)
        {
            ModelState.AddModelError("error", "Unable to find pocket !");
            return BadRequest(ModelState);
        }

        if (request.Amount > pocket.Balance)
        {
            ModelState.AddModelError("error", "The initial balance is greater than corresponding pocket balance !");
            return BadRequest(ModelState);
        }

        var result = await pocketService.MakeExpense(pocket, request.Amount);
        return Ok(result);
    }
}