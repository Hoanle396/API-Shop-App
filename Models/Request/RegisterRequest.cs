using System.ComponentModel.DataAnnotations;

namespace WebAPI.Models.Request
{
  public class RegisterRequest
  {
    [Required(ErrorMessage = "fullname is required field!")]
    public string? Fullname { get; set; }


    [EmailAddress, Required(ErrorMessage = "email is required field!")]
    public string? Email { get; set; }

    [Required(ErrorMessage = "pasword is required field!")]
    public string? Password { get; set; }
  }

}
