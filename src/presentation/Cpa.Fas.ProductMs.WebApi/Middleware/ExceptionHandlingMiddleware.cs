using Cpa.Fas.ProductMs.Application.Common.Exceptions;
using Cpa.Fas.ProductMs.Domain.Exceptions.Base;
using Microsoft.Data.SqlClient;
using System.Text.Json;

namespace Cpa.Fas.ProductMs.WebApi.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;
        private record ApiError(string PropertyName, string ErrorMessage);

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger) => (_logger, _next) = (logger, next);

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unhandled exception occurred: {Message}", ex.Message);
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = exception switch
            {
                BadRequestException or ValidationException => StatusCodes.Status400BadRequest,
                NotFoundException => StatusCodes.Status404NotFound,
                _ => StatusCodes.Status500InternalServerError
            };
            var message = "An unexpected error occurred.";
            var errors = new Dictionary<string, string[]>();

            switch (exception)
            {
                case ValidationException validationException:
                    message = "One or more validation errors occurred.";
                    errors = validationException.Errors.ToDictionary(
                        kvp => kvp.Key,
                        kvp => kvp.Value
                    );
                    break;

                case SqlException sqlException:
                    // Do we really want to expose SQL errors to the client?
                    // This is generally not recommended for production environments
                    message = "One or more database errors occurred.";
                    var sqlErrorDictionary = new Dictionary<string, string[]>();
                    var errorList = new List<string>();

                    for (int i = 0; i < sqlException.Errors.Count; i++)
                    {
                        errorList.Add(sqlException.Errors[i].ToString());
                    }
                    sqlErrorDictionary.Add("SqlErrors", errorList.ToArray());
                    errors = sqlErrorDictionary;
                    break;

                case ArgumentException:
                case InvalidOperationException:
                    message = exception.Message;
                    break;

                // Add more specific exception handling here if needed
                default:
                    break;
            }

            var response = new
            {
                StatusCode = context.Response.StatusCode,
                Message = message,
                Errors = errors.Any() ? errors : null
            };

            await context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
    }
}