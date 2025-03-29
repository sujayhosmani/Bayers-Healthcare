using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace BayersHealthcare.Common.ResponseInterceptor
{
    public static class GlobalExceptionHandler
    {
        public static void GlobalExceptionConfig(this IApplicationBuilder app)
        {
            app.UseExceptionHandler(appError =>
            {
                appError.Run(async context =>
                {
                    context.Response.ContentType = "application/json";
                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                    if (contextFeature != null)
                    {
                        var exception = contextFeature.Error;
                        if(exception is UnauthorizedAccessException)
                        {
                            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        }
                        else if(exception is NotImplementedException)
                        {
                            context.Response.StatusCode = StatusCodes.Status501NotImplemented;
                        }
                        else
                        {
                            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                        }

                        await context.Response.WriteAsync(new ErrorResponseBody(contextFeature.Error.Message, context.Response.StatusCode).ToString());
                    }
                });
            });
        }
    }
}
