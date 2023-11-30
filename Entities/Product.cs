namespace WebAPI.Entities;

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
public class Product : BaseEntity
{

  [Required, StringLength(100), Column("name")]
  public string? Name { get; set; }

  [Required, DefaultValue(0), Column("price")]
  public float Price { get; set; }

  [AllowNull, StringLength(5000), Column("description")]
  public string Description { get; set; }

  public int categoryId { get; set; }
  public Category? category { set; get; }

  public ICollection<Image>? Images { get; set; }
  public ICollection<Size>? Sizes { get; set; }
}