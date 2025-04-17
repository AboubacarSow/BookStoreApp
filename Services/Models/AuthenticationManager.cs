using AutoMapper;
using Entities.DataTransfertObjects.UserDtos;
using Entities.Exceptions;
using Entities.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Services.Contracts;
using System.IdentityModel.Tokens.Jwt;
using System.Security;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Services.Models
{
    public class AuthenticationManager : IAuthenticationService
    {
        private readonly ILoggerService _logger;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _configuration;
        private User? _user;
      
        public AuthenticationManager(ILoggerService logger, IMapper mapper, UserManager<User> userManager, IConfiguration configuration)
        {
            _logger = logger;
            _mapper = mapper;
            _userManager = userManager;
            _configuration = configuration;
        }

        public async Task<TokenDto> CreateTokenAsync(bool populateExpire)
        {
            var signIngCredentials = GetSignIngCredentials();
            var claims =await  GetClaims();
            var tokenOptions = GenerateTokenOptions(signIngCredentials, claims);

            var refreshToken = GenerateRefreshToken();
            _user.RefreshToken = refreshToken;
            if (populateExpire)
                _user.RefreshTokenExpireTime = DateTime.Now.AddDays(7);

            await  _userManager.UpdateAsync(_user);

            var accessToken= new JwtSecurityTokenHandler().WriteToken(tokenOptions);
            return new TokenDto() 
            {
                AccessToken = accessToken, 
                RefreshToken = refreshToken
            };
        }

        private JwtSecurityToken GenerateTokenOptions(SigningCredentials signIngCredentials,
            List<Claim> claims)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var securityToken = new JwtSecurityToken(
                issuer: jwtSettings["validIssuer"],
                audience: jwtSettings["validAudience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(Convert.ToDouble(jwtSettings["expires"])),
                signingCredentials: signIngCredentials);

            return securityToken;
      
        }

        private async Task<List<Claim>> GetClaims()
        {
            var claims = new List<Claim>()
            {
                new (ClaimTypes.Name,_user?.UserName!)
            };

            var roles = await _userManager.GetRolesAsync(_user!);
            foreach (var role in roles)
                claims.Add(new(ClaimTypes.Role, role));

            return claims;
        }

        private SigningCredentials GetSignIngCredentials()
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var key = Encoding.UTF8.GetBytes(jwtSettings["secretKey"]);
            var secret = new SymmetricSecurityKey(key);
            return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
        }

       

        public async Task<IdentityResult> RegisterAsync(UserForRegistrationDto userDto)
        {
            var user = _mapper.Map<User>(userDto);
            var result = await _userManager.CreateAsync(user, userDto.Password);
            if (result.Succeeded) await _userManager.AddToRolesAsync(user, userDto.Roles!);

            return result;
        }

        public async Task<bool> ValidateUserAsync(UserForValidationDto userForValidationDto)
        {
             _user = await _userManager.FindByNameAsync(userForValidationDto.UserName);
            var result = (_user != null && await _userManager.CheckPasswordAsync(_user, userForValidationDto.Password));
            if (!result)
                _logger
                    .LogWarning($"{nameof(ValidateUserAsync): Authentication failed.Wrong UserName or Password}");
            return result;
        }


        //Generate the refreshToken
        //It must be something brainer
        private string GenerateRefreshToken()
        {
            var randNumber = new byte[32];
            using var rnd = RandomNumberGenerator.Create();
            rnd.GetBytes(randNumber);
            return Convert.ToBase64String(randNumber);
        }
        private ClaimsPrincipal GetClaimsPrincipalFromExpiredToken(string token)
        {
            var jwtsettings = _configuration.GetSection("JwtSettings");
            string secretKey = jwtsettings["secretKey"]!;

            var tokenParameters = new TokenValidationParameters()
            {
                ValidateIssuer =true,
                ValidateAudience=true,
                ValidateIssuerSigningKey=true,
                ValidateLifetime=true,

                ValidIssuer = jwtsettings["validIssuer"],
                ValidAudience = jwtsettings["validAudience"],
                IssuerSigningKey=new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))               
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken;
            var principal = tokenHandler.ValidateToken(token, tokenParameters, out securityToken);
            var jwtSecurityToken = (JwtSecurityToken)securityToken;
            if(jwtSecurityToken is null ||
                !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            {
                throw new SecurityTokenException("Invalid Token!");
            }
            return principal;
        }

        public async Task<TokenDto> RefreshTokenAsync(TokenDto token)
        {
            var principal = GetClaimsPrincipalFromExpiredToken(token.AccessToken);
            var user = await _userManager.FindByNameAsync(principal.Identity.Name);

            if (user is null ||
                user.RefreshToken != token.RefreshToken ||
                user.RefreshTokenExpireTime<=DateTime.Now)
            {
                throw new RefreshTokenBadRequestException();
            }
            _user = user;
             return await CreateTokenAsync(populateExpire:false);           
        }
    }
}
