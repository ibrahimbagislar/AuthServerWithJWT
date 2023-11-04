using AuthServer.Core.DTOs;
using FluentValidation;

namespace AuthServer.API.Validations
{
    public class CreateUserDtoValidator : AbstractValidator<CreateUserDto>
    {
        public CreateUserDtoValidator()
        {
            RuleFor(x => x.Email).NotEmpty().WithMessage("Email boş olamaz.").EmailAddress().WithMessage("Geçerli bir email adresi giriniz.");
            RuleFor(x => x.UserName).NotEmpty().WithMessage("Kullanıcı adı boş olamaz.");
            RuleFor(x => x.Password).NotEmpty().WithMessage("Şifre boş olamaz.");
        }
    }
}
