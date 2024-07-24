using System.ComponentModel.DataAnnotations;

namespace ExpenseTracker.Api.Features.Transfers.Dtos;

public class InterPocketTransferDto
{
    [Required]
    public required Guid FromPocketId { get; set; }
    [Required]
    public required Guid ToPocketId { get; set; }
    [Required]
    public required double Amount { get; set; }
}