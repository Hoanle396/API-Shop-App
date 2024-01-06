using WebAPI.Entities;
using WebAPI.Enum;

namespace WebAPI.Models;

public class OrderProduct
{
  public Order order { get; set; }
  public Shipping shipping { get; set; }
  public string total { get; set; }
  public string discount { get; set; }
  public string finalPrice { get; set; }

  public List<ProductDetail>? product { get; set; }

}

public class ProductDetail
{
  public string Name { get; set; }
  public float? Price { get; set; }

  public int Quantity { get; set; }

  public float totalPrice { get; set; }

}