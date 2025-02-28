namespace Entities.Exceptions
{
    public abstract class BadRequestException : Exception 
    {
        protected BadRequestException(string message) : base(message) { }
    }
    public class PriceOutOfRangeBadRequestException : BadRequestException
    {
        public PriceOutOfRangeBadRequestException() 
            : base("Price should be less than 1000 and greater than 10.")
        {
        }
    }
}
