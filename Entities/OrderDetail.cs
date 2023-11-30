namespace WebAPI.Entities;

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using WebAPI.Enum;

public class OrderDetail : BaseEntity
{

  public int orderId { get; set; }
  public Order? order { get; set; }

  public int productId { get; set; }
  public Product? product { get; set; }

  [Required, StringLength(50), Column("size")]
  public string? Size { get; set; }

  [Required, Column("price")]
  public float Price { get; set; }

  [Required, Column("quantity")]
  public float Quantity { get; set; }

}