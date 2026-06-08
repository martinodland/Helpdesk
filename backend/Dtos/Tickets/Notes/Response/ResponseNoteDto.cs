namespace HelpDesk.Dtos.Tickets.Notes.Response;

public class ResponseNoteDto
{
    public required int Id { get; set; }
    
    public required string Description { get; set; }

    public required bool OnlyAdmin { get; set; }

    public required string WrittenByUser { get; set; }

    public required int UserId { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
}
