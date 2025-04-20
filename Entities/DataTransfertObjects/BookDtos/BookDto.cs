using Entities.DataTransfertObjects.CategoryDtos;

namespace Entities.DataTransfertObjects.BookDtos
{
    public record BookDto
    {
        public int id { get; init; }
        public string? Title { get; init; }
        public decimal Price { get; init ; }
        public CategoryDto Category { get; init; }
    }
}
