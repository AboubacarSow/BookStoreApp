namespace Entities.Exceptions
{
    public sealed class CategoryNotFoundException : NotFoundException
    {
        public CategoryNotFoundException(string id) :
            base($"Category with Id:{id} is not found")
        {
        }
    }
}
