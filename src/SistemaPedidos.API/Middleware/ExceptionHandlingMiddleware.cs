using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SistemaPedidos.Domain.Exceptions;

namespace SistemaPedidos.API.Middleware;

public sealed class ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception exception)
        {
            await WriteProblemAsync(context, exception);
        }
    }

    private async Task WriteProblemAsync(HttpContext context, Exception exception)
    {
        var (status, title, detail) = exception switch
        {
            EntityNotFoundException => (StatusCodes.Status404NotFound, "Registro no encontrado", exception.Message),
            BusinessConflictException => (StatusCodes.Status409Conflict, "Conflicto de negocio", exception.Message),
            ConcurrencyConflictException => (StatusCodes.Status409Conflict, "Conflicto de concurrencia", exception.Message),
            DbUpdateException => (StatusCodes.Status409Conflict, "Conflicto de persistencia", "La operación viola una restricción de datos."),
            _ => (StatusCodes.Status500InternalServerError, "Error interno", "Ocurrió un error inesperado.")
        };

        if (status == StatusCodes.Status500InternalServerError)
            logger.LogError(exception, "Error no controlado procesando {Method} {Path}", context.Request.Method, context.Request.Path);
        else
            logger.LogWarning(exception, "Solicitud rechazada con estado {StatusCode}", status);

        context.Response.StatusCode = status;
        await context.Response.WriteAsJsonAsync(new ProblemDetails
        {
            Status = status,
            Title = title,
            Detail = detail,
            Instance = context.Request.Path
        });
    }
}
