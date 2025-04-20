using System.ComponentModel.DataAnnotations;

namespace Entities.DataTransfertObjects.CategoryDtos
{
    public record CategoryManipulationDto
    {
        [Required(ErrorMessage ="Category Name field is required")]
        [MaxLength(5,ErrorMessage ="Category Name field must contain at least five characters ")]
        public string CategoryName { get; init; }
    }
}
