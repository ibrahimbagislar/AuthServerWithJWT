using AuthServer.Core.Configuration;
using AuthServer.Core.DTOs;
using AuthServer.Core.Entities;
using AuthServer.Core.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SharedLibrary.Configuration;
using SharedLibrary.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace AuthServer.Service.Services
{
    public class TokenService : ITokenService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly CustomTokenOptions _tokenOptions;

        public TokenService(UserManager<AppUser> userManager, IOptions<CustomTokenOptions> tokenOptions)
        {
            _userManager = userManager;
            _tokenOptions = tokenOptions.Value;
        }
        private string CreateRefreshToken()
        {
            var numberByte = new byte[32];
            using var random = RandomNumberGenerator.Create();
            random.GetBytes(numberByte);

            return Convert.ToBase64String(numberByte);
        }

        private IEnumerable<Claim> GetClaims(AppUser user, List<string> audiences)
        {
            var userClaimList = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier,user.Id),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.UserName)
            };

            userClaimList.AddRange(audiences.Select(x => new Claim(JwtRegisteredClaimNames.Aud, x)));
            return userClaimList;
        }

        private IEnumerable<Claim> GetClaimsByClient(Client client)
        {
            var clientClaims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub,client.ClientId),
            };
            if (client.Audiences != null)
                clientClaims.AddRange(client.Audiences.Select(x => new Claim(JwtRegisteredClaimNames.Aud, x)));
            return clientClaims;
        }

        public TokenDto CreateToken(AppUser appUser)
        {
            var accessTokenExpiration = DateTime.UtcNow.AddMinutes(_tokenOptions.AccessTokenExpirationMinute);
            var refreshTokenExpiration = accessTokenExpiration.AddMinutes(_tokenOptions.RefreshTokenExpirationMinute);
            var securityKey = SignService.GetSymmetricSecurityKey(_tokenOptions.SecurityKey);

            SigningCredentials credentials = new(securityKey, SecurityAlgorithms.HmacSha256Signature);

            JwtSecurityToken jwtSecurityToken = new
                (
                issuer: _tokenOptions.Issuer,
                expires: accessTokenExpiration,
                notBefore: DateTime.UtcNow,
                claims: GetClaims(appUser, _tokenOptions.Audience),
                signingCredentials: credentials
                );

            JwtSecurityTokenHandler handler = new();
            var token = handler.WriteToken(jwtSecurityToken);

            return new TokenDto
            {
                AccessToken = token,
                AccessTokenExpiration = accessTokenExpiration,
                RefreshToken = CreateRefreshToken(),
                RefreshTokenExpiration = refreshTokenExpiration
            };
        }

        public ClientTokenDto CreateTokenByClient(Client client)
        {
            var accessTokenExpiration = DateTime.UtcNow.AddMinutes(_tokenOptions.AccessTokenExpirationMinute);
            var securityKey = SignService.GetSymmetricSecurityKey(_tokenOptions.SecurityKey);

            SigningCredentials credentials = new(securityKey, SecurityAlgorithms.HmacSha256Signature);

            JwtSecurityToken jwtSecurityToken = new
                (
                issuer: _tokenOptions.Issuer,
                expires: accessTokenExpiration,
                notBefore: DateTime.UtcNow,
                claims: GetClaimsByClient(client),
                signingCredentials: credentials
                );

            JwtSecurityTokenHandler handler = new();
            var token = handler.WriteToken(jwtSecurityToken);

            return new ClientTokenDto
            {
               AccessToken = token,
               AccessTokenExpiration = accessTokenExpiration,
            };
        }
    }
}
