using AuthServer.Core.DTOs;
using AuthServer.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedLibrary.Exceptions;

namespace AuthServer.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : CustomBaseController
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }
        [HttpPost]
        public async Task<IActionResult> CreateUser(CreateUserDto dto)
        {
            return ActionResultInstance(await _userService.CreateUserAsync(dto));
        }
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetUserByUserName()
        {
            return ActionResultInstance(await _userService.GetUserByUserNameAsync(User.Identity.Name));
        }
    }
}
