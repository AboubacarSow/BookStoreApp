
namespace Entities.DataTransfertObjects.UserDtos
{
    public record TokenDto
    {
        public String AccessToken { get; init; }
        public String RefreshToken { get; init; }
    }
}
