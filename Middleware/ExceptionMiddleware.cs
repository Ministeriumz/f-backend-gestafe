using f_backend_gestafe.Middleware.Exceptions;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception ex)
    {
        context.Response.ContentType = "application/json";

        switch (ex)
        {
            case ValidationException ve:
                context.Response.StatusCode = 400;
                await context.Response.WriteAsJsonAsync(new
                {
                    Message = ve.Message,
                    Errors = ve.Errors
                });
                break;

            case ConflictException ce:
                context.Response.StatusCode = 409;
                await context.Response.WriteAsJsonAsync(new
                {
                    Field = ce.Field,
                    Message = ce.Message
                });
                break;

            case NotFoundException nfe:
                context.Response.StatusCode = 404;
                await context.Response.WriteAsJsonAsync(new
                {
                    Message = nfe.Message
                });
                break;

            default:
                context.Response.StatusCode = 500;
                await context.Response.WriteAsJsonAsync(new
                {
                    Message = ex.Message,
                    InnerException = ex.InnerException?.Message,
                    StackTrace = ex.StackTrace
                });
                break;
        }
    }
}