namespace WebAPI.Controllers;

using Microsoft.AspNetCore.Mvc;
using WebAPI.Enum;
using WebAPI.Helpers;
using WebAPI.Models;
using WebAPI.Models.Request;
using WebAPI.Services;

[ApiController]
[Route("user")]
public class UserController : ControllerBase
{
  private IUserService _userService;

  public UserController(IUserService userService)
  {
    _userService = userService;
  }

  [HttpGet(""),Authorize(Roles.ADMIN)]
  public async Task<IActionResult> Find([FromQuery] FindRequest model)
  {
    try
    {
      var response = await _userService.Find(model);
      return Ok(response);
    }
    catch (Exception ex)
    {
      System.Console.WriteLine(ex);
      return BadRequest(new { message = ex.Message });
    }
  }
  [HttpGet("{id}"),Authorize(Roles.ADMIN)]
  public async Task<IActionResult> FindById(int id)
  {
    try
    {
      var response = _userService.GetById(id);
      if (response == null)
      {
        return NotFound(new { message = "User does not exist." });
      }
      return Ok(response);
    }
    catch (Exception ex)
    {
      Console.WriteLine(ex);
      return NotFound(new { message = "User does not exist." });
    }
  }
  [HttpDelete("{id}")]
  [Authorize(Roles.ADMIN)]
  public async Task<IActionResult> DeleteById(int id)
  {
    try
    {
      var response = await _userService.DeleteById(id);
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
  public async Task<IActionResult> UpdateById(int id, [FromBody] UpdateUserRequest updateUser)
  {
    try
    {
      var response = await _userService.Update(updateUser, id);
      return Ok(response);
    }
    catch (Exception ex)
    {
      Console.WriteLine(ex);
      return BadRequest(new { message = ex.Message });
    }
  }
}