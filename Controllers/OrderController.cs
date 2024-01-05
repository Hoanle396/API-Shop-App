namespace WebAPI.Controllers;

using Microsoft.AspNetCore.Mvc;
using WebAPI.Entities;
using WebAPI.Enum;
using WebAPI.Helpers;
using WebAPI.Models;
using WebAPI.Models.Request;
using WebAPI.Services;

[ApiController]
[Route("order")]
public class OrderController : ControllerBase
{
  private IOrderService _orderService;

  public OrderController(IOrderService orderService)
  {
    _orderService = orderService;
  }

  [HttpPost("")]
  [Authorize]
  public async Task<IActionResult> Create(CreateOrderRequest model)
  {
    try
    {
      var response = await _orderService.Create(model, HttpContext.Items["User"] as User);

      if (response == null)
        return BadRequest(new { message = "can't create new order" });

      return Ok(response);
    }
    catch (Exception ex)
    {
      System.Console.WriteLine(ex);
      return BadRequest(new { message = ex.Message });
    }
  }

  [HttpGet(""), Authorize]
  public async Task<IActionResult> Find([FromQuery] FindRequest model)
  {
    try
    {
      var response = await _orderService.Find(model, HttpContext.Items["User"] as User);
      return Ok(response);
    }
    catch (Exception ex)
    {
      System.Console.WriteLine(ex);
      return BadRequest(new { message = ex.Message });
    }
  }
  [HttpGet("{id}"), Authorize]
  public async Task<IActionResult> FindById(int id)
  {
    try
    {
      var response = await _orderService.GetById(id);
      if (response == null)
      {
        return NotFound(new { message = "Order does not exist." });
      }
      return Ok(response);
    }
    catch (Exception ex)
    {
      Console.WriteLine(ex);
      return NotFound(new { message = "Order does not exist." });
    }
  }
  [HttpPost("{id}"), Authorize(Roles.ADMIN)]
  public async Task<IActionResult> Update(int id, UpdateOrderStatus status)
  {
    try
    {
      var response = await _orderService.Update(id, status);
      if (response == null)
      {
        return NotFound(new { message = "Order does not exist." });
      }
      return Ok(response);
    }
    catch (Exception ex)
    {
      Console.WriteLine(ex);
      return NotFound(new { message = "Order does not exist." });
    }
  }
}