namespace WebAPI.Models;

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

public class CreateProductRequest {
    [Required(ErrorMessage = "name is required field!")]
    public string? name { get; set; }

    [Required(ErrorMessage = "description is required field!")]
    public string description { get; set; }

    [Required(ErrorMessage = "price is required field!")]
    public float price { get; set; }

    [Required(ErrorMessage = "size is required field!")]
    public List<string> sizes { get; set; }

    [Required(ErrorMessage = "category is required field!")]
    public int category { get; set; }

    [Required(ErrorMessage = "images is required field!")]
    public List<IFormFile> images { set; get; }
}

public class Size {
    [Required(ErrorMessage = "sizename is required field!")]
    public string? sizeName { get; set; }

    [Required(ErrorMessage = "quantity is required field!")]
    public int quantity { get; set; }
}

public class FindRequest {

    [AllowNull, DefaultValue(1)]
    public int? page { get; set; }

    [AllowNull, DefaultValue(10)]
    public int? limit { get; set; }

    [AllowNull, DefaultValue("")]
    public string? keyword { get; set; }
}