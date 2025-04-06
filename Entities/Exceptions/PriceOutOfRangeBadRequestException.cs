namespace Entities.Exceptions
{
    public class PriceOutOfRangeBadRequestException : BadRequestException
    {
        public PriceOutOfRangeBadRequestException() 
            : base("Price should be less than 1000 and greater than 10.")
        {
        }
    }
}
