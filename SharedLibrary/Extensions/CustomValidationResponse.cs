
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using SharedLibrary.DTOs;

namespace SharedLibrary.Extensions
{
    //Bu sınıf fluentvalidation sonucunda clientlere gösterilecek hataların errordto şeklinde gösterilmesi gerektiğini içermektedir
    public static class CustomValidationResponse
    {
        public static void AddCustomValidationResponse(this IServiceCollection services)
        {
            services.Configure<ApiBehaviorOptions>(opt =>
            {
                // ModelState InValid olduğunda yani kullanıcıdan gelen modellerde bir hata olduğu durumda custom bir response oluşturuyoruz
                opt.InvalidModelStateResponseFactory = context =>
                {
                    var errors = context.ModelState.Values.Where(x => x.Errors.Count > 0).SelectMany(x=> x.Errors).Select(x => x.ErrorMessage);

                    var errorDto = new ErrorDto(errors.ToList(),true);
                    var response = Response<NoDataDto>.Fail(errorDto,400);
                    return new BadRequestObjectResult(response);
                };
            });
        }
    }
}
