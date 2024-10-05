using Domain.Utils.Helpers;
using Microsoft.AspNetCore.Authorization;
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
            var isAllowAnonymous = context.ActionDescriptor.EndpointMetadata
                             .Any(e => e.GetType() == typeof(AllowAnonymousAttribute));

            contextAccessor.SetSessionInfo(isAllowAnonymous);
        }
    }
}