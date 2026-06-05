namespace HelpDesk.Dtos.Tickets;

public class CreateTicketDto
{
    public required string Title { get; set; }

    public required string Text { get; set; }

    public required string Priority { get; set; }
}