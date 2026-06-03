namespace HelpDesk.Models;

public class RefreshToken
{
    public int Id { get; set; }

    public required string TokenHash { get; set; }

    public required int UserId { get; set; }

    public required User User { get; set; }

    public DateTime Expires { get; set; }

    public bool IsRevoked { get; set; } = false;
}