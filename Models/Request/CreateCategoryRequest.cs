namespace WebAPI.Models;

using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

public class CreateCategoryRequest
{
  [Required(ErrorMessage = "name is required field!")]
  public string? Name { get; set; }

  [AllowNull]
  public string? Description { get; set; }
}