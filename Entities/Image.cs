namespace WebAPI.Entities;

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

public class Image : BaseEntity {

    public int productId { get; set; }
    [JsonIgnore]
    public Product? product { get; set; }

    [Required, StringLength(1000), Column("url")]
    public string? Url { get; set; }
}