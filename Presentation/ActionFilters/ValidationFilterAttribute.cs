using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.ActionFilters
{
    public class ValidationFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var controller = context.RouteData.Values["controller"];//With this we get the controller 
            var action=context.RouteData.Values["action"];//Here we get the action
            var param=context.ActionArguments.SingleOrDefault(p=>p.Value.ToString().Contains("Dto")).Value;//Here we get the param

            if (param is null)
            {
                context.Result = new BadRequestObjectResult($"The current object is null:" +
                    $"controller:{controller}" +
                    $"action:{action}");
                return;
            }

            if (!context.ModelState.IsValid)
            {
                context.Result = new UnprocessableEntityObjectResult(context.ModelState);
                
            }
        }
    }
}
