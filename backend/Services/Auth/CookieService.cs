namespace HelpDesk.Services.Auth;

using HelpDesk.Models;

public class CookieService(ApplicationDbContext _dbContext, IHttpContextAccessor _context, JwtTokenService _jwtToken, RefreshTokenService _refreshToken)
{
    // Create cookies.

    public async Task<bool> CreateCookies(User User)
    {
        try
        {
            var jwtToken = await _jwtToken.CreateJwtToken(User.Id);

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
                UserId = User.Id,
                User = User,
                Expires = DateTime.UtcNow.AddDays(7)
            });
            

            return true;
        }
        catch
        {
            return false;
        }
    }
}