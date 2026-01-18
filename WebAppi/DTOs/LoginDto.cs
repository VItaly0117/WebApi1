using System.ComponentModel.DataAnnotations;

namespace WebAppi.DTOs;

public class LoginDto
{
    [Required]
    public string Login { get; set; }

    [Required]
    public string Password { get; set; }
}
