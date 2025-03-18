using FluentValidation;
using FluxoDeCaixa.Lancamento.Common.RequestResponse;
using FluxoDeCaixa.Lancamento.Common.Validation;
using System.Text.Json;

namespace FluxoDeCaixa.Lancamento.WebApi.Utils.Middleware;


public class InterceptExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<InterceptExceptionMiddleware> _logger;

    public InterceptExceptionMiddleware(RequestDelegate next, ILogger<InterceptExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }


    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (ValidationException ex)
        {
            _logger.LogError(ex, "Ocorreu um erro no ValidationException.");
            await HandleValidationExceptionAsync(context, ex, _logger);
        }
        catch (InvalidOperationException iEx)
        {
            _logger.LogError(iEx, "Ocorreu um erro no InvalidOperationException.");
            await HandleInvalidOperationExceptionAsync(context, iEx, _logger);
        }
        catch (KeyNotFoundException knfEx)
        {
            _logger.LogError(knfEx, "Ocorreu um erro no KeyNotFoundException.");
            await HandleKeyNotFoundExceptionAsync(context, knfEx, _logger);
        }
        catch (Exception exCore)
        {
            _logger.LogError(exCore, "Ocorreu um erro no Exception exCore.");
            await HandleKeyNotFoundExceptionAsync(context, exCore, _logger);
        }
    }

    private static Task HandleValidationExceptionAsync(HttpContext context, ValidationException exception, ILogger<InterceptExceptionMiddleware> logger)
    {
        return HandleExceptionAsync(context, validationException: exception, logger: logger);
    }
    private static Task HandleInvalidOperationExceptionAsync(HttpContext context, InvalidOperationException exception, ILogger<InterceptExceptionMiddleware> logger)
    {
        return HandleExceptionAsync(context, message: "Operation Failed", invalidOperationException: exception, logger: logger);
    }
    private static Task HandleKeyNotFoundExceptionAsync(HttpContext context, KeyNotFoundException exception, ILogger<InterceptExceptionMiddleware> logger)
    {
        return HandleExceptionAsync(context, message: "Key Found Failed", keyNotFoundException: exception, logger: logger);
    }
    private static Task HandleKeyNotFoundExceptionAsync(HttpContext context, Exception exception, ILogger<InterceptExceptionMiddleware> logger)
    {
        return HandleExceptionAsync(context, message: "Key Found Failed", exception: exception, logger: logger);
    }

    private static Task HandleExceptionAsync(HttpContext context, bool success = false, string message = "Validation Failed",
        ValidationException? validationException = null,
        InvalidOperationException? invalidOperationException = null,
        KeyNotFoundException? keyNotFoundException = null,
        Exception exception = null,
        ILogger<InterceptExceptionMiddleware> logger = null
        )
    {

        context.Response.ContentType = "application/json";

        IEnumerable<ValidationErrorDetail> listError = new List<ValidationErrorDetail>();
        if (validationException != null)
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            listError = validationException.Errors.Select(error => (ValidationErrorDetail)error);
            logger.LogError(validationException, "Result erro validationException");
        }

        if (invalidOperationException != null)
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            listError = new List<ValidationErrorDetail> { new ValidationErrorDetail() { Error = "OperationValidator", Detail = invalidOperationException.Message } };
            logger.LogError(invalidOperationException, "Result erro invalidOperationException");
        }

        if (keyNotFoundException != null)
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            listError = new List<ValidationErrorDetail> { new ValidationErrorDetail() { Error = "KeyFoundValidator", Detail = keyNotFoundException.Message } };
            logger.LogError(keyNotFoundException, "Result erro keyNotFoundException");
        }

        if (exception != null)
        {
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            listError = new List<ValidationErrorDetail> { new ValidationErrorDetail() { Error = "Error unexpected", Detail = exception.Message } };
            logger.LogError(exception, "Result erro exception");
        }

        var response = new ResponseRequest
        {
            Success = success,
            Message = message,
            Errors = listError
        };

        var jsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        var result = context.Response.WriteAsync(JsonSerializer.Serialize(response, jsonOptions));

        logger.LogError($"Result erro: {result}");

        return result;
    }
}
