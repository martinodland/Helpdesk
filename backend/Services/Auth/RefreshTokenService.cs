namespace HelpDesk.Services.Auth;

using System.Security.Cryptography;
using System.Text;

public class RefreshTokenService
{
    // Generate a random number with 64 bytes = 128 hex charachters (1 0-9 A-F).

    public string CreateRefreshToken()
    {
        return RandomNumberGenerator.GetHexString(64);
    }

    // Hash refresh token.

    public string HashRefreshToken(string refreshToken)
    {
        byte[] tokenInBytes = Encoding.UTF8.GetBytes(refreshToken);

        byte[] hashInBytes = SHA256.HashData(tokenInBytes);

        string hashedText = Convert.ToBase64String(hashInBytes);

        return hashedText;
    }
}