using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using System.Net;

namespace  TT.Deliveries.Web.Api.ErrorHandling;

/// <summary>
/// Middleware extensions for configuring exception logging
/// </summary>
public static class ExceptionMiddlewareExtensions
{
    /// <summary>
    /// Configure global exception handler to log unhandled exceptions
    /// </summary>
    /// <param name="app"></param>
    /// <param name="logger"></param>
    public static void ConfigureExceptionHandler(this IApplicationBuilder app, Serilog.ILogger logger)
    {
        app.UseExceptionHandler(appBuilder =>
        {
            appBuilder.Run(async context =>
            {
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                context.Response.ContentType = "application/json";

                var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                if (contextFeature != null)
                { 
                    logger.Error($"Unhandled error: {contextFeature.Error}");
                    await context.Response.WriteAsync(new ErrorResponce()
                    {
                        StatusCode = context.Response.StatusCode,
                        Message = "Internal Server Error"
                    }.ToString());
                }
            });
        });
    }
}
