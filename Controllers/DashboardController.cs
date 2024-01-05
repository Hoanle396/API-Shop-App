namespace WebAPI.Controllers;

using Microsoft.AspNetCore.Mvc;
using WebAPI.Entities;
using WebAPI.Helpers;
using WebAPI.Models;
using WebAPI.Models.Request;
using WebAPI.Services;

[ApiController]
[Route("dashboard")]
public class DashBoardController : ControllerBase
{
  private IDashboardService _dashboardService;

  public DashBoardController(IDashboardService dashboardService)
  {
    _dashboardService = dashboardService;
  }

  [HttpGet("statistical"), Authorize(Enum.Roles.ADMIN)]
  public async Task<IActionResult> Statistical()
  {
    var response = await _dashboardService.Statistical();
    return Ok(response);
  }

  [HttpGet("weekly-report"), Authorize(Enum.Roles.ADMIN)]
  public async Task<IActionResult> WeeklyReport()
  {
    var response = await _dashboardService.WeeklyReport();
    return Ok(response);
  }
}