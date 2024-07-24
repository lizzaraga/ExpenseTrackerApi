using System.ComponentModel.DataAnnotations;

namespace ExpenseTracker.Api.Features.Transfers.Dtos;

public class PursePocketTransferDto
{
    [Required]
    public required Guid PurseId { get; set; }
    [Required]
    public required Guid PocketId { get; set; }
    [Required]
    public required double Amount { get; set; }
}