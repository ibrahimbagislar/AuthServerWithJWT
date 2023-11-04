
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using SharedLibrary.DTOs;
using SharedLibrary.Exceptions;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SharedLibrary.Extensions
{
    public static class CustomExtensionHandler
    {
        public static void UseCustomException(this IApplicationBuilder app)
        {
            app.UseExceptionHandler(config =>
            {
                config.Run(async context =>
                {
                    context.Response.StatusCode = 500;
                    context.Response.ContentType = "application/json";

                    var errorFeature = context.Features.Get<IExceptionHandlerFeature>();

                    if (errorFeature != null)
                    {
                        var exception = errorFeature.Error;
                        ErrorDto errorDto;

                        // oluşan hata bizim custom olarak oluşturduğumuz hata ise kullanıcıya göster
                        if (exception is CustomException)
                            errorDto = new(exception.Message, true);
                        else
                            errorDto = new(exception.Message, false);

                        var response = Response<NoDataDto>.Fail(errorDto,500);
                        await context.Response.WriteAsync(JsonSerializer.Serialize(response, new JsonSerializerOptions
                        {
                            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                        }));
                    }
                });
            });
        }
    }
}
