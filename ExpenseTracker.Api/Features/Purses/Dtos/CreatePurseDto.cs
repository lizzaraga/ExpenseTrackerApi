namespace ExpenseTracker.Api.Features.Purses.Dtos;

public class CreatePurseDto
{
    public required string Currency { get; set; }
    public required string Name{ get; set; }
    public required double InitialBalance { get; set; }
}