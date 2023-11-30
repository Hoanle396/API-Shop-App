namespace WebAPI.Entities;

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using WebAPI.Enum;

public class User : BaseEntity
{

  [Required, StringLength(100), Column("fullname")]
  public string? FullName { get; set; }

  [Required, EmailAddress, Column("email")]
  public string? Email { get; set; }

  [Required, DefaultValue(Roles.USER), Column("role")]
  public Roles Role { get; set; }

  [Required, JsonIgnore, StringLength(256), Column("password")]
  public string? Password { get; set; }
}