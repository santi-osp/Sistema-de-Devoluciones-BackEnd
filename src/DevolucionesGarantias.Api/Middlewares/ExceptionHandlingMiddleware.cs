using System.Net;
using DevolucionesGarantias.Domain.Exceptions;
using DevolucionesGarantias.Shared.Results;

namespace DevolucionesGarantias.Api.Middlewares;

public sealed class ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (DuplicateRequestException ex)
        {
            await WriteErrorAsync(context, HttpStatusCode.Conflict, ex.Message);
        }
        catch (InvalidStateTransitionException ex)
        {
            await WriteErrorAsync(context, HttpStatusCode.BadRequest, ex.Message);
        }
        catch (UnauthorizedDomainActionException ex)
        {
            await WriteErrorAsync(context, HttpStatusCode.Forbidden, ex.Message);
        }
        catch (BusinessRuleException ex)
        {
            await WriteErrorAsync(context, HttpStatusCode.BadRequest, ex.Message);
        }
        catch (KeyNotFoundException ex)
        {
            await WriteErrorAsync(context, HttpStatusCode.NotFound, ex.Message);
        }
        catch (DomainException ex)
        {
            await WriteErrorAsync(context, HttpStatusCode.UnprocessableEntity, ex.Message);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unhandled exception while processing request {TraceId}.", context.TraceIdentifier);
            await WriteErrorAsync(context, HttpStatusCode.InternalServerError, "Ocurrio un error inesperado.");
        }
    }

    private static async Task WriteErrorAsync(HttpContext context, HttpStatusCode statusCode, string message)
    {
        context.Response.StatusCode = (int)statusCode;
        context.Response.ContentType = "application/json";

        var response = new
        {
            success = false,
            message,
            errors = Array.Empty<object>(),
            traceId = context.TraceIdentifier
        };

        await context.Response.WriteAsJsonAsync(response);
    }
}
