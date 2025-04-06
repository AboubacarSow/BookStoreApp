using Entities.DataTransfertObjects.UserDtos;
using Microsoft.AspNetCore.Identity;

namespace Services.Contracts
{
    public interface IAuthenticationService 
    {
        Task<IdentityResult> RegisterAsync(UserForRegistrationDto userDto);
        Task<bool> ValidateUserAsync(UserForValidationDto userForValidationDto);
        Task<TokenDto> CreateTokenAsync(bool populationExpire);
        Task<TokenDto> RefreshTokenAsync(TokenDto token);
    }
}
