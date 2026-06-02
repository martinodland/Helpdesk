namespace HelpDesk.Dtos.Auth;

public class LoginDto
{
    public required string Email { get; set; }

    public required string PasswordHash { get; set; }
}