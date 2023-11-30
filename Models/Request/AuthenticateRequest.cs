namespace WebAPI.Models;

using System.ComponentModel.DataAnnotations;

public class AuthenticateRequest {
    [EmailAddress, Required(ErrorMessage = "email is required field!")]
    public string? Email { get; set; }

    [Required(ErrorMessage = "pasword is required field!")]
    public string? Password { get; set; }
}