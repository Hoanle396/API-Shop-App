namespace WebAPI.Entities;

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

public class Discount : BaseEntity
{

  [Required, StringLength(50), Column("code")]
  public string? Code { get; set; }

  [Required, Column("type")]
  public WebAPI.Enum.Discount Type { get; set; }

  [Required, DefaultValue(0), Column("value")]
  public float Value { get; set; }

  [Required, StringLength(1000), Column("description")]
  public string? Description { get; set; }

  [Required, DatabaseGenerated(DatabaseGeneratedOption.Computed), Column("endDate")]
  public DateTime EndDate { get; set; }
}