using System.ComponentModel.DataAnnotations;
using WebAPI.Enum;

namespace WebAPI.Models;
public class CreateDiscountRequest
{
  [Required(ErrorMessage = "code is required field!")]
  public string code { get; set; }


  [Required(ErrorMessage = "description is required field!")]
  public string description { get; set; }

  [Required(ErrorMessage = "type is required field!")]
  public Discount type { get; set; }

  [Required(ErrorMessage = "value is required field!")]
  public float value { get; set; }

  [Required(ErrorMessage = "endDate is required field!")]
  public DateTime endDate { get; set; }

}