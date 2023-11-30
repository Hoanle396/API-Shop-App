namespace WebAPI.Controllers;

using Microsoft.AspNetCore.Mvc;
using WebAPI.Entities;
using WebAPI.Helpers;
using WebAPI.Models;
using WebAPI.Models.Request;
using WebAPI.Services;

[ApiController]
[Route("[controller]")]
public class authController : ControllerBase {
    private IUserService _userService;

    public authController(IUserService userService) {
        _userService = userService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Authenticate(AuthenticateRequest model) {
        var response = await _userService.Authenticate(model);

        if (response == null)
            return BadRequest(new { message = "Username or password is incorrect" });

        return Ok(response);
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequest model) {
        try {
            var user = await _userService.Register(model);
            return Ok(user);
        }
        catch (Exception ex) {
            await Console.Out.WriteLineAsync(ex.ToString());
            return BadRequest(new { message = ex.Message });
        }
    }

    [Authorize]
    [HttpGet("me")]
    public IActionResult GetAll() {
        var user = HttpContext.Items["User"] as User;
        return Ok(user);
    }
}