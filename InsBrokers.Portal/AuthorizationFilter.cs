using System;
using Elk.Core;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Controllers;

namespace Elk.AspNetCore.Mvc
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AuthorizationFilter2 : ActionFilterAttribute, IAuthorizationFilter
    {
        private bool IsAuthorized(AuthorizationFilterContext context, string controller, string action)
        {
            var id = context.HttpContext.User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
            var userActionList = (context.HttpContext.RequestServices.GetService(typeof(IUserActionProvider)) as IUserActionProvider).GetUserActions(id);
            if (userActionList == null || !userActionList.Any()) return false;
            return userActionList.Any(x => x.Controller.ToLower() == controller.ToLower() && x.Action.ToLower() == action.ToLower());
        }

        private async Task<bool> IsAuthorizedAsync(AuthorizationFilterContext context, string controller, string action)
        {
            var id = context.HttpContext.User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
            var userActionList = await (context.HttpContext.RequestServices.GetService(typeof(IUserActionProvider)) as IUserActionProvider).GetUserActionsAsync(id);
            if (userActionList == null || !userActionList.Any()) return false;
            return userActionList.Any(x => x.Controller.ToLower() == controller.ToLower() && x.Action.ToLower() == action.ToLower());
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (!context.HttpContext.User.Identity.IsAuthenticated)
            {
                context.Result = new StatusCodeResult(403);
                return;
            }

            var controllerActionDescriptor = context.ActionDescriptor as ControllerActionDescriptor;
            if (controllerActionDescriptor.MethodInfo.GetCustomAttributes(inherit: true).Any(a => a.GetType().Equals(typeof(AllowAnonymousAttribute)))) return;

            bool Authorize;
            if (controllerActionDescriptor.MethodInfo.GetCustomAttributes(inherit: true).Any(a => a.GetType().Equals(typeof(AuthEqualTo))))
            {
                var authEqualTo = (AuthEqualTo)controllerActionDescriptor.MethodInfo.GetCustomAttributes(typeof(AuthEqualTo), true).First();
                Authorize = IsAuthorized(context, authEqualTo.ControllerName, authEqualTo.ActionName);
            }
            else
                Authorize = IsAuthorized(context, context.RouteData.Values["controller"].ToString(), context.RouteData.Values["action"].ToString());

            if (!Authorize)
            {
                context.Result = new StatusCodeResult(401);
                return;
            }
        }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            if (!context.HttpContext.User.Identity.IsAuthenticated)
            {
                context.Result = new StatusCodeResult(403);
                return;
            }

            var controllerActionDescriptor = context.ActionDescriptor as ControllerActionDescriptor;
            if (controllerActionDescriptor.MethodInfo.GetCustomAttributes(inherit: true).Any(a => a.GetType().Equals(typeof(AllowAnonymousAttribute)))) return;

            bool Authorize;
            if (controllerActionDescriptor.MethodInfo.GetCustomAttributes(inherit: true).Any(a => a.GetType().Equals(typeof(AuthEqualTo))))
            {
                var authEqualTo = (AuthEqualTo)controllerActionDescriptor.MethodInfo.GetCustomAttributes(typeof(AuthEqualTo), true).First();
                Authorize = await IsAuthorizedAsync(context, authEqualTo.ControllerName, authEqualTo.ActionName);
            }
            else
                Authorize = await IsAuthorizedAsync(context, context.RouteData.Values["controller"].ToString(), context.RouteData.Values["action"].ToString());

            if (!Authorize)
            {
                context.Result = new StatusCodeResult(401);
                return;
            }
        }
    }
}
