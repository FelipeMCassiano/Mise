using System.ComponentModel.DataAnnotations;

public record RegisterUserDto([Required] string Username,
                              [Required][EmailAddress] string EmailAddress,
                              [Required][MinLength(6)] string Password);
