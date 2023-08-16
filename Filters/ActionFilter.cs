using System;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Microservice.Filters
{
    public class LogActionFilter : IActionFilter
    {
        private DateTime _startTime;

        public void OnActionExecuting(ActionExecutingContext context)
        {
            _startTime = DateTime.UtcNow;
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            var executionTime = DateTime.UtcNow - _startTime;
            var controllerName = context.Controller.GetType().Name;
            var actionName = context.ActionDescriptor.DisplayName;

            Console.WriteLine($"Action {actionName} in controller {controllerName} executed in {executionTime.TotalMilliseconds} ms");
        }
    }
}
