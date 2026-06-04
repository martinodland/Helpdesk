using HelpDesk.Dtos.Tickets;
using Microsoft.AspNetCore.Mvc;
using HelpDesk.Models;
using Microsoft.AspNetCore.Authorization;
using HelpDesk.Filters;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using HelpDesk.Dtos.Tickets.Response;

namespace HelpDesk.Controllers;

[ApiController]
[Route("tickets")]
public class TicketController(ApplicationDbContext _dbContext): ControllerBase
{
    // Get user tickets.

    [HttpGet]
    [Authorize]
    [CsrfHeader]
    public async Task<ActionResult> GetTickets()
    {
        int userId = int.Parse(User.FindFirstValue("id")!);

        string userRole = User.FindFirstValue("role")!;

        List<Ticket> tickets;

        if(userRole == "User")
        {
            tickets = await _dbContext.Tickets.Where(searchedTicket => searchedTicket.CreadtedByUserId == userId).ToListAsync();

            return Ok ( new { message = "Found user tickets successfully!", tickets = tickets});
        }

        tickets = await _dbContext.Tickets.ToListAsync();

        return Ok( new { message = "Found all tickets successfully!", tickets = tickets });
    }

    // Create a ticket.

    [HttpPost]
    [Authorize]
    [CsrfHeader]
    public async Task<ActionResult> CreateTicket(CreateTicketDto dto)
    {
        int userId = int.Parse(User.FindFirstValue("id")!);

        await _dbContext.Tickets.AddAsync(new Ticket
        {
            Title = dto.Title,
            Description = dto.Text,
            Status = "Åpen",
            Priority = dto.Priority,
            CreadtedByUserId = userId
        });

        await _dbContext.SaveChangesAsync();

        return Ok( new { message = "Ticket was created successfully!"});
    }

    // Update ticket.

    [HttpPatch("{id}")]
    [Authorize]
    [CsrfHeader]
    public async Task<ActionResult> UpdateTicket(int id, UpdateTicketDto dto)
    {
        Ticket? ticket = await _dbContext.Tickets.FirstOrDefaultAsync(searchedTicket => searchedTicket.Id == id);

        if ( ticket is null ) return NotFound(new { message = "Ticket does not exist "});

        int userId = int.Parse(User.FindFirstValue("id")!);

        string userRole = User.FindFirstValue("role")!;

        if (userRole == "User" && userId != ticket.CreadtedByUserId) return StatusCode(403, new { message = "You dont have access to this ticket!"} );

        bool updated = false;

        if (dto.Status is not null)
        {
            ticket.Status = dto.Status;

            updated = true;
        }

        if (dto.Title is not null)
        {
            ticket.Title = dto.Title;

            updated = true;
        }

        if(dto.Description is not null)
        {
            ticket.Description = dto.Description;

            updated = true;
        }

        if (updated)
        {
            ticket.UpdatedAt = DateTime.UtcNow;
        }

        await _dbContext.SaveChangesAsync();

        return Ok(new { message = "Ticket was updated successfully "});
    }

    // Delete ticket.

    [HttpDelete("{id}")]
    [Authorize]
    [CsrfHeader]
    public async Task<ActionResult> DeleteTicket(int id)
    {
        string userRole = User.FindFirstValue("role")!;

        if(userRole != "Admin")
        {
            return StatusCode(403, new { message = "You dont have access to delete this ticket!"} );
        }

        Ticket? ticket = await _dbContext.Tickets.FirstOrDefaultAsync(searchedTicket => searchedTicket.Id == id);

        if (ticket is null)
        {
            return NotFound(new { message = "Ticket does not exist." });
        }

        _dbContext.Remove(ticket);

        await _dbContext.SaveChangesAsync();

        return Ok(new { message = "Ticket successfully deleted!" });
    }

    // Get a specific ticket
    [HttpGet("{id}")]
    [Authorize]
    [CsrfHeader]
    public async Task<ActionResult> GetTicket(int id)
    {
        string userRole = User.FindFirstValue("role")!;

        Ticket? ticket;

        if(userRole != "Admin")
        {
            int userId = int.Parse(User.FindFirstValue("id")!);

            ticket = await _dbContext.Tickets.Include(searchedTicket => searchedTicket.CreatedByUser).FirstOrDefaultAsync(searchedTicket => searchedTicket.Id == id && searchedTicket.CreadtedByUserId == userId);

            if(ticket is null)
            {
                return NotFound(new { message = "Ticket does not exist." });
            }

            return Ok(new { message = "Ticket retrieved successfully!", ticket = MapToDto(ticket)});
        }

        ticket = await _dbContext.Tickets.Include(searchedTicket => searchedTicket.CreatedByUser).FirstOrDefaultAsync(searchedTicket => searchedTicket.Id == id);

        if(ticket is null)
        {
            return NotFound(new { message = "Ticket does not exist." });
        }

        return Ok(new { message = "Ticket retrieved successfully!", ticket = MapToDto(ticket) }); 
    }

    private static ResponseTicketDto MapToDto(Ticket ticket) => new()
    {
        Id = ticket.Id,
        Title = ticket.Title,
        Description = ticket.Description,
        Status = ticket.Status,
        Priority = ticket.Priority,
        CreatedByUser = ticket.CreatedByUser,
        CreatedAt = ticket.CreatedAt
    };
}