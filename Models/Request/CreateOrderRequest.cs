using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using WebAPI.Enum;

namespace WebAPI.Models;

public class CreateOrderRequest{

  [Required(ErrorMessage = "phone is required field!")]
  public string phone { get; set; }

  [Required(ErrorMessage = "email is required field!")]
  public string email { get; set; }

  [Required(ErrorMessage = "address is required field!")]
  public string address { get; set; }

  [AllowNull]
  public string? discount { get; set; }

  [Required(ErrorMessage = "products is required field!")]
  public List<Products> products { get; set; }
}

public class Products{
  [Required(ErrorMessage = "product is required field!")]
  public int product { get; set; }

  [Required(ErrorMessage = "size is required field!")]
  public string size { get; set; }

  [Required(ErrorMessage = "quantity is required field!")]
  public int quantity { get; set; }
}

public class UpdateOrderStatus{
  [Required(ErrorMessage = "status is required field!")]
  public OrderStatus status { get; set; }
}

public class UpdatePaymentStatus
{
  [Required(ErrorMessage = "status is required field!")]
  public PaymentStatus status { get; set; }
}