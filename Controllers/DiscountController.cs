namespace WebAPI.Controllers;

using Microsoft.AspNetCore.Mvc;
using WebAPI.Enum;
using WebAPI.Helpers;
using WebAPI.Models;
using WebAPI.Models.Request;
using WebAPI.Services;

[ApiController]
[Route("discount")]
public class DiscountController : ControllerBase
{
  private IDiscountService _discountService;

  public DiscountController(IDiscountService discountService)
  {
    _discountService = discountService;
  }

  [HttpPost("")]
  [Authorize(Roles.ADMIN)]
  public async Task<IActionResult> Create(CreateDiscountRequest model)
  {
    try
    {
      var response = await _discountService.Create(model);

      if (response == null)
        return BadRequest(new { message = "can't create new discount" });

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
      var response = await _discountService.Find(model);
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
      var response = await _discountService.GetById(id);
      if (response == null)
      {
        return NotFound(new { message = "Discount does not exist." });
      }
      return Ok(response);
    }
    catch (Exception ex)
    {
      Console.WriteLine(ex);
      return NotFound(new { message = "Discount does not exist." });
    }
  }

  [HttpGet("check/{code}")]
  public async Task<IActionResult> Check(string code)
  {
    try
    {
      var response = await _discountService.Check(code);
      return Ok(response);
    }
    catch (Exception ex)
    {
      Console.WriteLine(ex);
      return BadRequest(new { message = ex.Message });
    }
  }

  [HttpDelete("{id}")]
  [Authorize(Roles.ADMIN)]
  public async Task<IActionResult> DeleteById(int id)
  {
    try
    {
      var response = await _discountService.DeleteById(id);
      return Ok(response);
    }
    catch (Exception ex)
    {
      Console.WriteLine(ex);
      return BadRequest(new { message = ex.Message });
    }
  }
  [HttpGet("work")]
  public async Task<IActionResult> FindWork([FromQuery] FindRequest request)
  {
    try
    {
      var response = await _discountService.FindWork(request);
      return Ok(response);
    }
    catch (Exception ex)
    {
      Console.WriteLine(ex);
      return BadRequest(new { message = ex.Message });
    }
  }
}