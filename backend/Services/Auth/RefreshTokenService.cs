namespace HelpDesk.Services.Auth;

using System.Security.Cryptography;

public class RefreshTokenService
{

    // Generate a random number with 32 bytes (00001010101010+)
    public string CreateRefreshToken()
    {
        return RandomNumberGenerator.GetHexString(64);
    }
}