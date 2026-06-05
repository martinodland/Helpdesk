namespace HelpDesk.Dtos.Tickets.Query;

public sealed record GetTicketsQueryDto
{
    public string? Status { get; init; }
} 