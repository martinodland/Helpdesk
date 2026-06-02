using HelpDesk.Dtos.Auth;
using HelpDesk.Models;
using HelpDesk.Services.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HelpDesk.Controllers;

[ApiController]
[Route("auth")]
public class AuthController(ApplicationDbContext _dbContext, CookieService _cookie): ControllerBase
{
    [HttpPost]
    [Authorize]
    [Route("test")]
    public async Task<ActionResult> test()
    {
        return Ok(new { message = "success!"});
    }

    [HttpPost]
    [Route("register")]
    public async Task<ActionResult> register(RegisterDto dto)
    {
        if ( await _dbContext.Users.AnyAsync(searchedUser => searchedUser.Email == dto.Email)) return Conflict(new { message = "Email already in use." });

        var User = new User
        {
            Name = dto.Name,
            Email = dto.Email,
            PasswordHash = new PasswordHasher<User>().HashPassword(null!, dto.Password),
            Role = "User"
        };

        await _dbContext.Users.AddAsync(User);

        await _dbContext.SaveChangesAsync();

        if(!await _cookie.CreateCookies(User)) return Unauthorized(new {  message = "Couldnt store cookies." });

        await _dbContext.SaveChangesAsync();

        return Ok(new { message = "User created successfully!"});
    }
}