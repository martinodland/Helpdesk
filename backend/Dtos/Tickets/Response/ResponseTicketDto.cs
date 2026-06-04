using HelpDesk.Models;

namespace HelpDesk.Dtos.Tickets.Response;

public class ResponseTicketDto
{
     public int Id { get; set; }

    public required string Title { get; set; }

    public required string Description { get; set; }

    public required string Status { get; set; }

    public required string Priority { get; set; }

    public User? CreatedByUser { get; set; }

    public required DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public DateTime? ClosedAt { get; set; }
}