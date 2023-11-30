namespace WebAPI.Entities;

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using WebAPI.Enum;

public class Order : BaseEntity
{

  [Required, Column("code")]
  public string? Code { get; set; }

  public int userId { get; set; }
  public User? user { get; set; }

  [Required, Column("amount")]
  public float Amount { get; set; }

  [Required, DefaultValue(OrderStatus.PENDING), Column("status")]
  public OrderStatus Status { get; set; }
}