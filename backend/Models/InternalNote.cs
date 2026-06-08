namespace HelpDesk.Models;

public class InternalNote
{
    public int Id { get; set; }

    public required int TicketId { get; set; }

    public Ticket? Ticket { get; set; }

    public User? User { get; set; }

    public required int UserId { get; set; }

    public required string Description { get; set; }

    public required bool OnlyAdmin { get; set; } = false;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? UpdatedAt { get; set; }
}