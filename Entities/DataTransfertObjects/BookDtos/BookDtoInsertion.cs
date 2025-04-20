namespace Entities.DataTransfertObjects.BookDtos
{
    public record BookDtoInsertion :BookDtoManipulation
    {
        public int CategoryId { get; init; }
    }
}
