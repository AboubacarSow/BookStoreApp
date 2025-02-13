using System.ComponentModel.DataAnnotations;

namespace Entities.DataTransfertObjects
{
    public record BookDtoUpdate : BookDtoManipulation
    {
        [Required] public int id { get; set; }

    }
}
