namespace Entities.Exceptions
{
    public sealed class BookBadRequestException : BadRequestException
    {
        public BookBadRequestException(string? message = null)
            : base(message?? "ID FORMATI GEÇERSİZDİR")
        {
        }
    }

}
