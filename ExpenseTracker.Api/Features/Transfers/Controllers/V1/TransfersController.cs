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
            ModelState.AddModelError("error", "The amount is greater than corresponding purse balance !");
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
    
    [HttpPost("PocketToPurse")]
    public async Task<ActionResult<PursePocketTransfer>> TransferFromPocketToPurse(
        [FromBody] PursePocketTransferDto request
    )
    {
        var purse = dbContext.Purses.FirstOrDefault(x => x.Id.Equals(request.PurseId));
        if (purse is null)
        {
            ModelState.AddModelError("error", "Purse not found");
            return BadRequest(ModelState);
        }
        
        var pocket = dbContext.Pockets.FirstOrDefault(x => x.Id.Equals(request.PocketId) && x.PurseId.Equals(request.PurseId));
        if (pocket is null)
        {
            ModelState.AddModelError("error", "Pocket not found");
            return BadRequest(ModelState);
        }
        
        if (request.Amount > pocket.Balance)
        {
            ModelState.AddModelError("error", "The amount is greater than corresponding pocket balance !");
            return BadRequest(ModelState);
        }

        var result = await transferService.MakePocketToPurseTransfer(pocket, purse, request.Amount);
        return result.Item3;
    }
    
    [HttpPost("PocketToPocket")]
    public async Task<ActionResult<PocketPocketTransfer>> TransferFromPocketToPocket(
        [FromBody] InterPocketTransferDto request
    )
    {
        var fromPocket = dbContext.Pockets.FirstOrDefault(x => x.Id.Equals(request.FromPocketId));
        if (fromPocket is null)
        {
            ModelState.AddModelError("error", "Pocket emitter not found");
            return BadRequest(ModelState);
        }
        
        if (request.Amount > fromPocket.Balance)
        {
            ModelState.AddModelError("error", "The amount is greater than corresponding pocket emitter balance !");
            return BadRequest(ModelState);
        }
        
        var toPocket = dbContext.Pockets.FirstOrDefault(x => x.Id.Equals(request.ToPocketId));
        if (toPocket is null)
        {
            ModelState.AddModelError("error", "Pocket receiver not found");
            return BadRequest(ModelState);
        }
        if (!fromPocket.PurseId.Equals(toPocket.PurseId))
        {
            ModelState.AddModelError("error", "Your pockets don't come from the same purse");
            return BadRequest(ModelState);
        }
        
        
        var purse = dbContext.Purses.FirstOrDefault(x => x.Id.Equals(fromPocket.PurseId));
        if (purse is null)
        {
            ModelState.AddModelError("error", "Purse not found");
            return BadRequest(ModelState);
        }
        
        

        var result = await transferService.MakePocketToPocketTransfer(purse, fromPocket, toPocket, request.Amount);
        return result.Item3;
    }
}