using System.Net;
using System.Security.Authentication;
using System.Text.Json;
using AgroControlUI.Exceptions;
using AgroControlUI.Services.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using AspNetCoreGeneratedDocument;

namespace AgroControlUI.Middleware
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILoggerService _logger;

        public ErrorHandlingMiddleware(RequestDelegate next, ILoggerService logger)
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
            catch (Exception ex)
            {
                _logger.LogError("An error occurred while processing the request.", ex);
                await HandleExceptionAsync(context, ex);
            }
        }

        private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var statusCode = HttpStatusCode.InternalServerError;
            string message = "An unexpected error occurred.";
            string details = exception.Message;


            if (exception is HttpRequestException httpEx && httpEx.StatusCode == HttpStatusCode.Unauthorized)
            {
                statusCode = HttpStatusCode.Unauthorized;
                message = "Authentication failed. Please log in.";
                await context.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                var tempData = context.Items["TempData"] as ITempDataDictionary;
                //tempData.Add("errorMessage", "You are not authorized to view this page. Please log in.");
                context.Response.Redirect("/Account/Login");
                return;
            }
            context.Response.Redirect("/Home");
            return;
        }
    }
}
