using System.ComponentModel.DataAnnotations;

namespace ExpenseTracker.Api.Features.Pockets.Dtos;

public class CreatePocketDto
{
    [Required]
    public required Guid PurseId { get; set; }

    [Required]
    public required string Name { get; set; }

    public required double InitialBalance { get; set; }
}