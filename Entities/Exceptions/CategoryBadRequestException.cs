namespace Entities.Exceptions
{
    public sealed class CategoryBadRequestException : BadRequestException
    {
        public CategoryBadRequestException(string message) : base(message?? "Wrong format id")
        {
        }
    }
}
