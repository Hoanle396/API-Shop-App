namespace WebAPI.Controllers;

using Azure.Core;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Nodes;
using WebAPI.Enum;
using WebAPI.Helpers;
using WebAPI.Models;
using WebAPI.Models.Request;
using WebAPI.Services;

[ApiController]
[Route("product")]
public class ProductController : ControllerBase {
    private IProductService _productService;

    public ProductController(IProductService productService) {
        _productService = productService;
    }

    [Authorize(Roles.ADMIN)]
    [HttpPost("")]
    public async Task<IActionResult> Create([FromForm] CreateProductRequest model) {
        try {
            List<Size> sizes = new List<Size>();
            var validationErrors = new List<string>();
            foreach (var obj in model.sizes) {
                var jsonObject = JsonConvert.DeserializeObject<Models.Size>(obj);
                var validationContext = new ValidationContext(jsonObject, serviceProvider: null, items: null);
                var validationResults = new List<ValidationResult>();
                bool isValid = Validator.TryValidateObject(jsonObject, validationContext, validationResults, validateAllProperties: true);

                if (!isValid) {
                    var errors = validationResults.Select(v => v.ErrorMessage);
                    validationErrors.AddRange(errors);
                }
                sizes.Add(jsonObject);
            }

            if (validationErrors.Any()) {
                return BadRequest(validationErrors);
            }
            var response = await _productService.CreateProduct(model,sizes);
            return Ok(response);
        } catch (Exception ex) {
            System.Console.WriteLine(ex);
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet("")]
    public async Task<IActionResult> Find([FromQuery] FindRequest model) {
        try {
            var response = await _productService.Find(model);
            return Ok(response);
        } catch (Exception ex) {
            System.Console.WriteLine(ex);
            return BadRequest(new { message = ex.Message });
        }
    }
    [HttpGet("{id}")]
    public async Task<IActionResult> FindById(int id) {
        try {
            var response = await _productService.GetById(id);
            return Ok(response);
        } catch (Exception ex) {
            Console.WriteLine(ex);
            return NotFound(new { message = "Product does not exist." });
        }
    }
    [HttpDelete("{id}")]
    [Authorize(Roles.ADMIN)]
    public async Task<IActionResult> DeleteById(int id) {
        try {
            var response = await _productService.DeleteById(id);
            return Ok(response);
        } catch (Exception ex) {
            Console.WriteLine(ex);
            return BadRequest(new { message = ex.Message});
        }
    }
}