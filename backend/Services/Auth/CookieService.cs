namespace HelpDesk.Services.Auth;

using HelpDesk.Models;

public class CookieService(ApplicationDbContext _dbContext, HttpContext _context, JwtTokenService _jwtToken, RefreshTokenService _refreshToken)
{
    public async Task<bool> CreateCookies(User User)
    {
        var jwtToken = await _jwtToken.CreateJwtToken(User.Id);

        var refreshToken = _refreshToken.CreateRefreshToken();

        // Jwt

        _context.Response.Cookies.Append("accessToken", jwtToken, new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict,
            Expires = DateTimeOffset.UtcNow.AddMinutes(15)
        });

        // Refresh

        _context.Response.Cookies.Append("refreshToken", refreshToken, new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict,
            Expires = DateTimeOffset.UtcNow.AddDays(7)
        });

        // Add refresh to db

        await _dbContext.RefreshTokens.AddAsync(new RefreshToken
        {
            Token = refreshToken,
            UserId = User.Id,
            User = User,
            Expires = DateTime.UtcNow.AddDays(7)
        });

        return true;
    }
}