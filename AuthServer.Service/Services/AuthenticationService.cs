using AuthServer.Core.Configuration;
using AuthServer.Core.DTOs;
using AuthServer.Core.Entities;
using AuthServer.Core.Repositories;
using AuthServer.Core.Services;
using AuthServer.Core.UnitOfWork;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SharedLibrary.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthServer.Service.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly List<Client> _clients;
        private readonly ITokenService _tokenService;
        private readonly UserManager<AppUser> _userManager;
        private readonly IUnitOfWork _uow;
        private readonly IGenericRepository<UserRefreshToken> _refreshTokenRepository;

        public AuthenticationService(IOptions<List<Client>> optionsClient, ITokenService tokenService, UserManager<AppUser> userManager, IUnitOfWork uow, IGenericRepository<UserRefreshToken> refreshTokenRepository)
        {
            _clients = optionsClient.Value;
            _tokenService = tokenService;
            _userManager = userManager;
            _uow = uow;
            _refreshTokenRepository = refreshTokenRepository;
        }

        public async Task<Response<TokenDto>> CreateTokenAsync(LoginDto loginDto)
        {
            if (loginDto == null)
                throw new ArgumentNullException(nameof(loginDto));

            var user = await _userManager.FindByEmailAsync(loginDto.Email);

            //kullanıcı var mı
            if (user == null)
                return Response<TokenDto>.Fail("Email or password is wrong", 400);

            //şifresi doğru mu
            if (!await _userManager.CheckPasswordAsync(user, loginDto.Password))
                return Response<TokenDto>.Fail("Email or password is wrong", 400);

            //kullanıcı var ve şifresi doğru o zaman yeni bir token oluşturuyoruz
            var token = _tokenService.CreateToken(user);

            //ilgili kullanıcının bir refresh tokeni yok ise kullanıcıya yeni bir refresh token veriyoruz eğer var ise refresh tokenini yeniliyoruz
            var userRefreshToken = await _refreshTokenRepository.GetByFilter(x => x.UserId == user.Id).SingleOrDefaultAsync(x => x.RefreshToken == token.RefreshToken);
            if (userRefreshToken == null)
                await _refreshTokenRepository.AddAsync(new UserRefreshToken { UserId = user.Id, Expiration = token.RefreshTokenExpiration, RefreshToken = token.RefreshToken });
            else
            {
                userRefreshToken.RefreshToken = token.RefreshToken;
                userRefreshToken.Expiration = token.RefreshTokenExpiration;
            }

            await _uow.SaveChangesAsync();
            return Response<TokenDto>.Success(token, 200);
        }

        public Response<ClientTokenDto> CreateTokenByClientAsync(ClientLoginDto dto)
        {
            if (dto == null)
                return Response<ClientTokenDto>.Fail("Failed request", 400);

            var client = _clients.SingleOrDefault(x => x.ClientId == dto.ClientId && x.ClientSecret == dto.ClientSecret);
            if (client == null)
                return Response<ClientTokenDto>.Fail("Failed ClientId", 400);

            // bu aşamada artık client id ve secret doğru oluyor ve ilgili tokeni oluşturuyoruz.
            var token = _tokenService.CreateTokenByClient(client);
            return Response<ClientTokenDto>.Success(token, 200);

        }

        public async Task<Response<TokenDto>> CreateTokenByRefreshTokenAsync(string refreshToken)
        {
            var userRefreshToken = await _refreshTokenRepository.GetByFilter(x => x.RefreshToken == refreshToken).SingleOrDefaultAsync();

            if (userRefreshToken == null)
                return Response<TokenDto>.Fail("RefreshToken Not Found", 404);

            if (userRefreshToken.Expiration > DateTime.UtcNow)
                return Response<TokenDto>.Fail("Refresh token expiration has not occurred.", 400);


            var user = await _userManager.FindByIdAsync(userRefreshToken.UserId);
            if (user == null)
                return Response<TokenDto>.Fail("User Not Found", 404);
            var token = _tokenService.CreateToken(user);
            userRefreshToken.RefreshToken = token.RefreshToken;
            userRefreshToken.Expiration = token.RefreshTokenExpiration;

            await _uow.SaveChangesAsync();

            return Response<TokenDto>.Success(token, 200);

        }

        public async Task<Response<NoDataDto>> RevokeRefreshTokenAsync(string refreshToken)
        {
            var userRefreshToken = await _refreshTokenRepository.GetByFilter(x => x.RefreshToken == refreshToken).SingleOrDefaultAsync();

            if (userRefreshToken == null)
                return Response<NoDataDto>.Fail("RefreshToken Not Found", 404);

            _refreshTokenRepository.Remove(userRefreshToken);
            await _uow.SaveChangesAsync();

            return Response<NoDataDto>.Success(200);
        }
    }
}
