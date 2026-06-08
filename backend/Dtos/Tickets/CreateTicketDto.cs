using System.ComponentModel.DataAnnotations;

namespace HelpDesk.Dtos.Tickets;

public class CreateTicketDto
{
    [Required]
    public required string Title { get; set; }

    [Required]
    public required string Text { get; set; }

    [Required]
    public required string Priority { get; set; }
}