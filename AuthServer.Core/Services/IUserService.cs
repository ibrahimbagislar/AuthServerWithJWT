
using AuthServer.Core.DTOs;
using SharedLibrary.DTOs;

namespace AuthServer.Core.Services
{
    public interface IUserService
    {
        Task<Response<AppUserDto>> CreateUserAsync(CreateUserDto dto);
        Task<Response<AppUserDto>> GetUserByUserNameAsync(string userName);

    }
}
