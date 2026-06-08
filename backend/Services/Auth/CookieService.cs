namespace HelpDesk.Services.Auth;

using HelpDesk.Models;
using Microsoft.EntityFrameworkCore;

public class CookieService(ApplicationDbContext _dbContext, IHttpContextAccessor _context, JwtTokenService _jwtToken, RefreshTokenService _refreshToken)
{
    // Create cookies.

    public async Task<bool> CreateCookies(User user)
    {
        try
        {
            var jwtToken = await _jwtToken.CreateJwtToken(user.Id, user.Role);

            _context.HttpContext.Response.Cookies.Append("accessToken", jwtToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTimeOffset.UtcNow.AddMinutes(15)
            });
        
            var refreshToken = _refreshToken.CreateRefreshToken();

            _context.HttpContext.Response.Cookies.Append("refreshToken", refreshToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTimeOffset.UtcNow.AddDays(7)
            });

            await _dbContext.RefreshTokens.AddAsync(new RefreshToken
            {
                TokenHash = _refreshToken.HashRefreshToken(refreshToken),
                UserId = user.Id,
                Expires = DateTime.UtcNow.AddDays(7)
            });
            

            return true;
        }
        catch
        {
            return false;
        }
    }

    // Destroy cookies and remove refreshtoken from db.

    public async Task<bool> DestroyCookies(int userId)
    {
        try
        {
            _context.HttpContext.Response.Cookies.Delete("accessToken");

            _context.HttpContext.Response.Cookies.Delete("refreshToken");

            RefreshToken? refreshToken = await _dbContext.RefreshTokens.FirstOrDefaultAsync(searchedRefreshToken => searchedRefreshToken.UserId == userId);

            if( refreshToken is null)
            {
                return true;
            }

            _dbContext.Remove(refreshToken);

            await _dbContext.SaveChangesAsync();

            return true;
        }
        catch
        {
            return false;
        }
    }
}