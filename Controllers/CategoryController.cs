namespace WebAPI.Controllers;

using Microsoft.AspNetCore.Mvc;
using WebAPI.Enum;
using WebAPI.Helpers;
using WebAPI.Models;
using WebAPI.Models.Request;
using WebAPI.Services;

[ApiController]
[Route("category")]
public class CategoryController : ControllerBase
{
  private ICategoryService _categoryService;

  public CategoryController(ICategoryService categoryService)
  {
    _categoryService = categoryService;
  }

  [HttpPost("")]
  [Authorize(Roles.ADMIN)]
  public async Task<IActionResult> Create(CreateCategoryRequest model)
  {
    try
    {
      var response = await _categoryService.CreateCategory(model);

      if (response == null)
        return BadRequest(new { message = "can't create new category" });

      return Ok(response);
    }
    catch (Exception ex)
    {
      System.Console.WriteLine(ex);
      return BadRequest(new { message = ex.Message });
    }
  }

  [HttpGet("")]
  public async Task<IActionResult> Find([FromQuery] FindRequest model)
  {
    try
    {
      var response = await _categoryService.Find(model);
      return Ok(response);
    }
    catch (Exception ex)
    {
      System.Console.WriteLine(ex);
      return BadRequest(new { message = ex.Message });
    }
  }
  [HttpGet("{id}")]
  public async Task<IActionResult> FindById(int id)
  {
    try
    {
      var response = await _categoryService.GetById(id);
      if (response == null)
      {
        return NotFound(new { message = "Category does not exist." });
      }
      return Ok(response);
    }
    catch (Exception ex)
    {
      Console.WriteLine(ex);
      return NotFound(new { message = "Category does not exist." });
    }
  }
  [HttpDelete("{id}")]
  [Authorize(Roles.ADMIN)]
  public async Task<IActionResult> DeleteById(int id)
  {
    try
    {
      var response = await _categoryService.DeleteById(id);
      return Ok(response);
    }
    catch (Exception ex)
    {
      Console.WriteLine(ex);
      return BadRequest(new { message = ex.Message });
    }
  }
  [HttpPatch("{id}")]
  [Authorize(Roles.ADMIN)]
  public async Task<IActionResult> UpdateById(int id, [FromBody] CreateCategoryRequest categoryRequest)
  {
    try
    {
      var response = await _categoryService.Update(categoryRequest, id);
      return Ok(response);
    }
    catch (Exception ex)
    {
      Console.WriteLine(ex);
      return BadRequest(new { message = ex.Message });
    }
  }
}