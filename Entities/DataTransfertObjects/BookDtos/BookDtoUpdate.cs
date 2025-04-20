using System.ComponentModel.DataAnnotations;

namespace Entities.DataTransfertObjects.BookDtos
{
    public record BookDtoUpdate : BookDtoManipulation
    {
        public int id { get; set; }
        public int CategoryId { get; init; } 

    }
}
