namespace WebAPI.Entities;

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using WebAPI.Enum;

public class Shipping : BaseEntity
{

  public int orderId { get; set; }
  public Order? order { get; set; }

  [Required, DefaultValue(PaymentStatus.PENDING), Column("status")]
  public PaymentStatus PaymentStatus { get; set; }

  [Required, StringLength(1000), Column("address")]
  public string? Address { get; set; }

  [Required, StringLength(255), Column("email")]
  public string? Email { get; set; }

}