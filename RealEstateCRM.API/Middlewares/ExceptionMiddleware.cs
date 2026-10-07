using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace RealEstateCRM.API.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;
        private readonly IHostEnvironment _env;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger, IHostEnvironment env)
        {
            _next = next;
            _logger = logger;
            _env = env;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                // Pass request to the next middleware in the pipeline
                await _next(context);
            }
            catch (Exception ex)
            {
                // Log unhandled exception details
                _logger.LogError(ex, "An unhandled exception occurred: {Message}", ex.Message);

                await HandleExceptionAsync(context, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/problem+json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            // Standard RFC 7807 ProblemDetails object
            var problem = new ProblemDetails
            {
                Status = context.Response.StatusCode,
                Title = "An unhandled server error occurred.",
                Detail = _env.IsDevelopment() ? exception.Message : "A server error occurred. Please try again later.",
                Instance = context.Request.Path
            };

            // Add StackTrace in Development mode for easier debugging
            if (_env.IsDevelopment())
            {
                problem.Extensions["stackTrace"] = exception.StackTrace;
            }

            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            var json = JsonSerializer.Serialize(problem, options);

            return context.Response.WriteAsync(json);
        }
    }
}