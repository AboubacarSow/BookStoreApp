using Entities.DataTransfertObjects.UserDtos;
using Microsoft.AspNetCore.Mvc;
using Presentation.ActionFilters;
using Services.Contracts;


namespace Presentation.Controllers
{
    [ApiExplorerSettings(GroupName ="v1")]
    [ApiController]
    [Route("api/authentication")]
    public class AuthenticationController : ControllerBase
    {
        private readonly IServiceManager _manager;

        public AuthenticationController(IServiceManager manager)
        {
            _manager = manager;
        }
        [HttpPost("register")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> Register([FromBody] UserForRegistrationDto userDto)
        {
            var result=await _manager.AuthenticationService.RegisterAsync(userDto);
            if (!result.Succeeded)
            {
                foreach(var error in result.Errors)
                {
                    ModelState.TryAddModelError(error.Code, error.Description);
                }
                return BadRequest(ModelState);
            }
            return StatusCode(201);
        }
        [HttpPost("login")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> Login([FromBody] UserForValidationDto userDto)
        {
            if (!await _manager.AuthenticationService.ValidateUserAsync(userDto))
                return Unauthorized();
            var token = await _manager
                .AuthenticationService
                .CreateTokenAsync(true);
            return Ok(token);
        }

        [HttpPost("refresh")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> Refresh([FromBody]TokenDto tokenDto)
        {
            var token = await _manager.AuthenticationService.RefreshTokenAsync(tokenDto);
            return Ok(token);
        }
    }
}
