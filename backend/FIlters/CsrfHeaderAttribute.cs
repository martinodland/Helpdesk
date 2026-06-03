using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace HelpDesk.Filters;

public class CsrfHeaderAttribute() : ActionFilterAttribute
{
    // Check for X-CSRF
    public override void OnActionExecuting(ActionExecutingContext _context)
    {
        if (!_context.HttpContext.Request.Headers.ContainsKey("X-CSRF"))
        {
            _context.Result = new UnauthorizedObjectResult( new
            {
                message = "Missing CSRF header."
            });

            return;
        }

        base.OnActionExecuting(_context);
    }
}