using FluentValidation;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Net;

namespace UserManagementProject.Infrastructure.Middlewares
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context); // Pass the request to the next middleware
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            var response = context.Response;
            var errorResponse = new { message = "An unexpected error occurred", details = "" };

            switch (exception)
            {
                case ValidationException validationException: // FluentValidation Errors
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    errorResponse = new
                    {
                        message = "Validation failed",
                        details = string.Join("; ", validationException.Errors.Select(e => e.ErrorMessage)) // Convert list to a single string
                    };
                    break;

                case KeyNotFoundException: // Not Found Error
                    response.StatusCode = (int)HttpStatusCode.NotFound;
                    errorResponse = new
                    {
                        message = exception.Message,
                        details = "The requested resource was not found"
                    };
                    break;

                default: // General Server Error
                    response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    errorResponse = new
                    {
                        message = "An unexpected error occurred",
                        details = "Something went wrong on the server"
                    };
                    break;
            }

            var result = JsonConvert.SerializeObject(errorResponse);
            return context.Response.WriteAsync(result);
        }
    }
}
