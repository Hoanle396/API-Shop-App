namespace WebAPI.Entities;

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

public class Size : BaseEntity
{

  public int productId { get; set; }
  [JsonIgnore]
  public Product? product { get; set; }

  [Required, StringLength(50), Column("size")]
  public string? SizeName { get; set; }

  [Required, DefaultValue(0), Column("quantity")]
  public int Quantity { get; set; }
}