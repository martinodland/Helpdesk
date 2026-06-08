using System.ComponentModel.DataAnnotations;

namespace HelpDesk.Dtos.Auth;

public class RegisterDto
{
    [Required]
    public required string Name { get; set;}

    [Required]
    public required string Email { get; set; }

    [Required]
    [MinLength(8)]
    public required string Password { get; set; }
}