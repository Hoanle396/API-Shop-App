using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace WebAPI.Models.Request
{
  public class UpdateUserRequest
  {
    [AllowNull]
    public string? FullName { get; set; }


    [EmailAddress, AllowNull]
    public string? Email { get; set; }
  }

}
