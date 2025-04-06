using System.ComponentModel.DataAnnotations;

namespace Entities.DataTransfertObjects.BookDtos
{
    public record BookDtoUpdate : BookDtoManipulation
    {
        [Required] public int id { get; set; }

    }
}
