using ExpenseTracker.Api.Features.Transfers.Dtos;
using ExpenseTracker.Api.Features.Transfers.Interfaces;
using ExpenseTracker.Database;
using ExpenseTracker.Database.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseTracker.Api.Features.Transfers.Controllers.V1;

[Authorize]
[ApiController]
[Route("Api/[controller]")]
public class TransfersController(
    ILogger<TransfersController> logger,
    ITransferService transferService,
    ExpenseTrackerDbContext dbContext
    ): ControllerBase
{
    [HttpPost("PurseToPocket")]
    public async Task<ActionResult<PursePocketTransfer>> TransferFromPurseToPocket(
        [FromBody] PursePocketTransferDto request
        )
    {
        var purse = dbContext.Purses.FirstOrDefault(x => x.Id.Equals(request.PurseId));
        if (purse is null)
        {
            ModelState.AddModelError("error", "Purse not found");
            return BadRequest(ModelState);
        }

        if (request.Amount > purse.Balance)
        {
            ModelState.AddModelError("error", "The initial balance is greater than corresponding purse balance !");
            return BadRequest(ModelState);
        }
        
        var pocket = dbContext.Pockets.FirstOrDefault(x => x.Id.Equals(request.PocketId) && x.PurseId.Equals(request.PurseId));
        if (pocket is null)
        {
            ModelState.AddModelError("error", "Pocket not found");
            return BadRequest(ModelState);
        }

        var result = await transferService.MakePurseToPocketTransfer(pocket, purse, request.Amount);
        return result.Item3;
    }
}