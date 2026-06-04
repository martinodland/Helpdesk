using System.Security.Claims;
using HelpDesk.Dtos.Auth;
using HelpDesk.Filters;
using HelpDesk.Models;
using HelpDesk.Services.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HelpDesk.Controllers;

[ApiController]
[Route("auth")]
public class AuthController(ApplicationDbContext _dbContext, CookieService _cookie, IHttpContextAccessor _context, RefreshTokenService _refreshToken): ControllerBase
{
    // Get logged in user.

    [HttpGet]
    [Authorize]
    [Route("me")]
    public async Task<ActionResult> me()
    {
        int userId = int.Parse(User.FindFirstValue("id")!);

        User? user = await _dbContext.Users.FirstOrDefaultAsync(searchedUser => searchedUser.Id == userId);

        return Ok(new { message = "success!", user = user});
    }

    // Login the user.

    [HttpPost]
    [Route("login")]
    [CsrfHeader]
    public async Task<ActionResult> Login(LoginDto dto)
    {
        User? user = await _dbContext.Users.FirstOrDefaultAsync(searchedUser => searchedUser.Email == dto.Email);

        if (user is null) return Unauthorized(new { message = "Invalid credentials." });

        var checkHash = new PasswordHasher<User>().VerifyHashedPassword(user, user.PasswordHash, dto.Password);

        if (checkHash == PasswordVerificationResult.Failed) return Unauthorized(new { message = "Invalid credentials." });
        
        if(!await _cookie.CreateCookies(user)) return Unauthorized(new {  message = "Couldnt store cookies." });

        await _dbContext.SaveChangesAsync();

        return Ok(new { message = "User successfully logged in!" });
    }

    // Register the user.

    [HttpPost]
    [Route("register")]
    [CsrfHeader]
    public async Task<ActionResult> Register(RegisterDto dto)
    {
        if (await _dbContext.Users.AnyAsync(searchedUser => searchedUser.Email == dto.Email)) return Conflict(new { message = "Email already in use." });

        var user = new User
        {
            Name = dto.Name,
            Email = dto.Email,
            PasswordHash = new PasswordHasher<User>().HashPassword(null!, dto.Password),
            Role = "User"
        };

        await _dbContext.Users.AddAsync(user);

        // Dette er ikke bra med 2 stk, men kan fikse hvis jeg har tid.

        await _dbContext.SaveChangesAsync();

        if(!await _cookie.CreateCookies(user)) return Unauthorized(new {  message = "Couldnt store cookies." });

        await _dbContext.SaveChangesAsync();

        return Ok(new { message = "User created successfully!"});
    }

    // Refresh endpoint.

    [HttpPost]
    [Route("refresh")]
    [CsrfHeader]
    public async Task<ActionResult> Refresh()
    {
        string? refreshToken = _context.HttpContext?.Request.Cookies["refreshToken"];

        if (string.IsNullOrWhiteSpace(refreshToken))
        {
            return Unauthorized( new { message = "Refresh token not valid."});
        }

        var storedToken = await _dbContext.RefreshTokens.FirstOrDefaultAsync(searchedRefreshToken => searchedRefreshToken.TokenHash == _refreshToken.HashRefreshToken(refreshToken));

        if ( storedToken is not null )
        {
            if(storedToken.Expires > DateTime.UtcNow)
            {
                User? user = await _dbContext.Users.FirstOrDefaultAsync(searchedUser => searchedUser.Id == storedToken.UserId);

                if (user is null)
                {
                    _dbContext.RefreshTokens.Remove(storedToken);

                    await _dbContext.SaveChangesAsync();

                    return Unauthorized( new { message = "User is not valid."});
                }
                
                _dbContext.RefreshTokens.Remove(storedToken);

                await _cookie.CreateCookies(user);

                await _dbContext.SaveChangesAsync();

                return Ok(new { message = "Cookies successfully set!"});
                     
            }
            else
            {
                _dbContext.RefreshTokens.Remove(storedToken);

                await _dbContext.SaveChangesAsync();
            }        
        }

        return Unauthorized(new { message = "Refresh token or user not valid." });
    }

}