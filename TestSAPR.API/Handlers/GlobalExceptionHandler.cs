using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using TestSAPR.Domain.Exceptions;

namespace TestSAPR.API.Handlers
{
    public class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            if (exception is not BaseDomainException domainEx)
                return false;

            logger.LogWarning(exception, "Domain error: {Message}", domainEx.Message);

            if (exception is PartAlreadyExists)
            {
                httpContext.Response.StatusCode = StatusCodes.Status409Conflict;
                await httpContext.Response.WriteAsJsonAsync(new ProblemDetails
                {
                    Status = domainEx.StatusCode,
                    Title = "Business Rule Violation",
                    Detail = domainEx.Message,
                    Instance = httpContext.Request.Path
                }, cancellationToken);

                return true; 
            }

         

            return true; 
        }

    }
}
