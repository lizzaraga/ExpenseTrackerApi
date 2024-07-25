using System.ComponentModel.DataAnnotations;

namespace ExpenseTracker.Api.Features.Pockets.Dtos;

public class PocketExpenseDto
{
    [Required] public required Guid PocketId { get; set; }
    [Required] public required double Amount { get; set; }
}