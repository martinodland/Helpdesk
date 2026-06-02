namespace HelpDesk.Models;

public class InternalNote
{
    public int Id { get; set; }

    public required int TicketId { get; set; }

    public required int AdminUserId { get; set; }

    public required string Note { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}