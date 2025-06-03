using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace MovieApp.API.Filters
{
    public class ValidationFilter : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                var errors = context.ModelState
                    .Where(kvp => kvp.Value.Errors.Any())
                    .ToDictionary(
                        kvp => kvp.Key,
                        kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray()
                    );

                var responseObj = new
                {
                    Message = "Validation Failed",
                    Errors = errors
                };

                context.Result = new BadRequestObjectResult(responseObj);
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
        }
    }
}
