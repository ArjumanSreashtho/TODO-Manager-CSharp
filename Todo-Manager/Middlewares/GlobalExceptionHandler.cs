using System.Net;
using Todo_Manager.Helper;

namespace Todo_Manager.Middlewares;

public class GlobalExceptionHandler : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (CustomException error)
        {
            context.Response.StatusCode = error.ErrorCode;
            await context.Response.WriteAsJsonAsync(new
            {
                success = false,
                message = error.Message
            });
        }
    }
}