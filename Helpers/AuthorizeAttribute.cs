namespace WebAPI.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using WebAPI.Entities;
using WebAPI.Enum;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class AuthorizeAttribute : Attribute, IAuthorizationFilter
{
  public Roles? role;
  public AuthorizeAttribute(Roles _role)
  {
    role = _role;
  }
  public AuthorizeAttribute() { }

  public void OnAuthorization(AuthorizationFilterContext context)
  {
    var user = context.HttpContext.Items["User"] as User;
    if (user == null)
    {
      context.Result = new JsonResult(new { message = "401 Unauthorized Access" }) { StatusCode = StatusCodes.Status401Unauthorized };
    }
    else if (role != null && role != user.Role)
    {
      context.Result = new JsonResult(new { message = "You do not have permission to access this URL" }) { StatusCode = StatusCodes.Status403Forbidden };
    }
  }
}