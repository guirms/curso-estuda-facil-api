using Domain.Utils.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Presentation.Web.Filters
{
    public class ActionFilter(IHttpContextAccessor contextAccessor) : ControllerBase, IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            contextAccessor.SaveTokens();
        }
    }
}