using AuthServer.Core.DTOs;
using AuthServer.Core.Services;
using Microsoft.AspNetCore.Mvc;

namespace AuthServer.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthController : CustomBaseController
    {
        private readonly IAuthenticationService _authenticationService;

        public AuthController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }
        [HttpPost]
        public async Task<IActionResult> CreateToken(LoginDto dto)
        {
            var response = await _authenticationService.CreateTokenAsync(dto);
            return ActionResultInstance(response);
        }
        [HttpPost]
        public IActionResult CreateTokenByClient(ClientLoginDto dto)
        {
            var response = _authenticationService.CreateTokenByClientAsync(dto);
            return ActionResultInstance(response);
        }
        [HttpPost]
        public async Task<IActionResult> RevokeRefreshToken(RefreshTokenDto dto)
        {
            var response = await _authenticationService.RevokeRefreshTokenAsync(dto.RefreshToken);
            return ActionResultInstance(response);
        }
        [HttpPost] 
        public async Task<IActionResult> CreateTokenByRefreshToken(RefreshTokenDto dto)
        {
            var response = await _authenticationService.CreateTokenByRefreshTokenAsync(dto.RefreshToken);
            return ActionResultInstance(response);
        }
    }
}
