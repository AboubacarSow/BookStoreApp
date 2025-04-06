namespace Entities.DataTransfertObjects.BookDtos
{
    public record BookDto
    {
        public int id { get; set; }
        public string? Title { get; set; }
        public decimal Price { get; set; }
    }
}
