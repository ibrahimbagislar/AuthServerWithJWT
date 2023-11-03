using AuthServer.Core.DTOs;
using AuthServer.Core.Entities;
using AuthServer.Core.Services;
using AuthServer.Service.Mappings;
using Microsoft.AspNetCore.Identity;
using SharedLibrary.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthServer.Service.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<AppUser> _userManager;

        public UserService(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<Response<AppUserDto>> CreateUserAsync(CreateUserDto dto)
        {
            var user = new AppUser { Email = dto.Email, UserName = dto.UserName, };
            var result = await _userManager.CreateAsync(user, dto.Password);

            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(x => x.Description);
                return Response<AppUserDto>.Fail(new ErrorDto(errors.ToList(),true),400);
            }
            return Response<AppUserDto>.Success(ObjectMapper.Mapper.Map<AppUserDto>(user),200);
        }

        public async Task<Response<AppUserDto>> GetUserByNameAsync(string userName)
        {
            var user = await _userManager.FindByNameAsync(userName);
            if (user == null)
                return Response<AppUserDto>.Fail("User Not Found", 404);
            return Response<AppUserDto>.Success(ObjectMapper.Mapper.Map<AppUserDto>(user),200);
        }
    }
}
