using Microsoft.AspNetCore.Mvc.Filters;
using FastxWebApi.Models.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace FastxWebApi.Filter
{
    public class CustomExceptionFilter:ExceptionFilterAttribute
    {
        private readonly ILogger<CustomExceptionFilter> _logger;

        public CustomExceptionFilter(ILogger<CustomExceptionFilter> logger)
        {
            _logger = logger;
        }
        public override void OnException(ExceptionContext context)
        {
            
            _logger.LogError(context.Exception, "An unhandled exception occurred.");

            var errorObject = new ErrorObjectDTOcs
            {
                ErrorMessage = "An unexpected error occurred. Please try again later.",
                ErrorNumber = 500
            };

            context.Result = new ObjectResult(errorObject)
            {
                StatusCode = 500
            };

            context.ExceptionHandled = true;
        }
    }
}
