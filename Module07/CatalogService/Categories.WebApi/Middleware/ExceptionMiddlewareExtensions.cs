using System.Collections.Concurrent;
using System.Net;
using System.Text;
using System.Text.Json;
using Categories.Application.Common.Exceptions;
using Categories.WebApi.Models.Output;
using Microsoft.AspNetCore.Diagnostics;

namespace ordermanagement.shared.order_api.Extensions
{
    public static class ExceptionMiddlewareExtensions
    {
        private static readonly ConcurrentDictionary<Type, (HttpStatusCode statusCode, bool canOutputMessage)> _exceptionStatusCodeMap = new();

        static ExceptionMiddlewareExtensions()
        {
            _exceptionStatusCodeMap.TryAdd(typeof(OutOfMemoryException), (HttpStatusCode.InternalServerError, false));
            _exceptionStatusCodeMap.TryAdd(typeof(NotFoundException), (HttpStatusCode.NotFound, true));
        }
        
        private static (string message, HttpStatusCode statusCode) GetMessageAndStatus(Exception exception)
        {
            string message = null;
            HttpStatusCode statusCode = HttpStatusCode.InternalServerError;
            if (_exceptionStatusCodeMap.TryGetValue(exception.GetType(), out var value))
            {
                if(value.canOutputMessage)
                {
                    message = exception.Message;
                }

                statusCode = value.statusCode;
            }

            if(string.IsNullOrEmpty(message))
            {
                message = "An unknown exception has occured.";
            }

            return (message, statusCode);
        }

        public static void ConfigureExceptionHandler(this IApplicationBuilder app)
        {
            app.UseExceptionHandler(appError =>
            {
                appError.Run(async context =>
                {
                    context.Response.ContentType = "application/json";

                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                    if (contextFeature != null)
                    {
                        var (message, statusCode) = GetMessageAndStatus(contextFeature.Error);

                        context.Response.StatusCode = (int)statusCode;
                        await context.Response.WriteAsync(JsonSerializer.Serialize(new OutputErrorDataModel(
                            new OutputErrorDataType
                            {
                                Message = message
                            })
                        ), Encoding.UTF8);
                    }
                });
            });
        }
    }
}
