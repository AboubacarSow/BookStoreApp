using System.ComponentModel.DataAnnotations;

namespace Entities.DataTransfertObjects.UserDtos
{
    public record UserForValidationDto
    {
        [Required(ErrorMessage ="UserName is required.")]
        public string UserName { get; init; }
        [Required(ErrorMessage="Password is required")]
        public string Password { get; init; }
    }
}
