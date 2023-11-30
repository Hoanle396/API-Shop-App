namespace WebAPI.Entities;

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
public class Category : BaseEntity
{
  [Required, StringLength(255), Column("name")]
  public string? Name { get; set; }

  [StringLength(1000), Column("description")]
  public string? Description { get; set; }
}