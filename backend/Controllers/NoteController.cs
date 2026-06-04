using HelpDesk.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using HelpDesk.Models;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using HelpDesk.Dtos.Tickets.Notes;
using HelpDesk.Dtos.Tickets.Notes.Response;

namespace HelpDesk.Controllers;

[ApiController]
[Route("tickets/{id}/notes")]
public class NoteController(ApplicationDbContext _dbContext): ControllerBase
{
    // Get notes on ticket.

    [HttpGet]
    [Authorize]
    [CsrfHeader]
    public async Task<ActionResult> GetNotesOnTicket(int id)
    {
        string userRole = User.FindFirstValue("role")!;

        List<InternalNote> notes;

        if(userRole == "User")
        {
            notes = await _dbContext.InternalNotes.Where(searchedNote => searchedNote.TicketId == id && searchedNote.OnlyAdmin == false).Include(searchedUser => searchedUser.User).ToListAsync();

            return Ok(new { message = "All public notes retrieved successfully!", notes = notes.Select(MapToDto) });
        }

        notes = await _dbContext.InternalNotes.Where(searchedNote => searchedNote.TicketId == id).Include(searchedUser => searchedUser.User).ToListAsync();

        return Ok(new { message = "All notes retrieved successfully!", notes = notes.Select(MapToDto) });
    }

    // Create note on ticket.

    [HttpPost]
    [Authorize]
    [CsrfHeader]
    public async Task<ActionResult> CreateNoteOnTicket(int id, CreateNoteDto dto)
    {   
        int userId = int.Parse(User.FindFirstValue("id")!);

        await _dbContext.InternalNotes.AddAsync(new InternalNote
        {
            TicketId = id,
            UserId = userId,
            Description = dto.Description,
            OnlyAdmin = dto.OnlyAdmin
        });

        await _dbContext.SaveChangesAsync();

        return Ok(new { message = "New note successfully created on ticket!" });
    }

    // Edit note on ticket.

    [HttpPatch("{noteId}")]
    [Authorize]
    [CsrfHeader]
    public async Task<ActionResult> EditNoteOnTicket(int id, int noteId, EditNoteDto dto)
    {
        int userId = int.Parse(User.FindFirstValue("id")!);

        InternalNote? note = await _dbContext.InternalNotes.FirstOrDefaultAsync(searchedNote => searchedNote.TicketId == id && searchedNote.Id == noteId && searchedNote.UserId == userId);

        if( note is null ) return NotFound(new { message = "Internal note does not exist or was not written by this user." });

        bool updated = false;

        if(dto.Description is not null)
        {
            note.Description = dto.Description;

            updated = true;
        }

        if(updated)
        {
            note.UpdatedAt = DateTime.UtcNow;

            await _dbContext.SaveChangesAsync();
        }

        return Ok(new { message = "Note successfully updated!"});
    }

    // Delete note on ticket.

    [HttpDelete("{noteId}")]
    [Authorize]
    [CsrfHeader]
    public async Task<ActionResult> DeleteNoteOnTicket(int id, int noteId)
    {
        int userId = int.Parse(User.FindFirstValue("id")!);

        InternalNote? note = await _dbContext.InternalNotes.FirstOrDefaultAsync(searchedNote => searchedNote.TicketId == id && searchedNote.Id == noteId && searchedNote.UserId == userId);

        if( note is null ) return NotFound(new { message = "Internal note does not exist or was not written by this user." });

        _dbContext.InternalNotes.Remove(note);

        await _dbContext.SaveChangesAsync();

        return Ok(new { message = "Note was successfully deleted!" });
    }

    private static ResponseNoteDto MapToDto(InternalNote note) => new()
    {
        Id = note.Id,
        Description = note.Description,
        OnlyAdmin = note.OnlyAdmin,
        UserId = note.UserId,
        WrittenByUser = note.User?.Name ?? "Unknown",
        CreatedAt = note.CreatedAt,
        UpdatedAt = note.UpdatedAt
    };
}