namespace HelpDesk.Dtos.Tickets.Notes;

public class CreateNoteDto
{
    public required string Description { get; set; }

    public bool IsPrivate { get; set; } = false;
}