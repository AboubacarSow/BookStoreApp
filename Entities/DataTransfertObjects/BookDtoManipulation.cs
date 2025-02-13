using System.ComponentModel.DataAnnotations;

namespace Entities.DataTransfertObjects
{
    public abstract record BookDtoManipulation
    {
        [Required(ErrorMessage ="Title field is required")]
        [MinLength(2,ErrorMessage ="Title must containt at least 2 characters")]
        [MaxLength(50,ErrorMessage ="Title must containt maximum 50 characters")]
        public string Title {  get; set; }
        [Required(ErrorMessage ="Price field is required")]
        [Range(10,1000)]
        public decimal Price { get; set; }
    }
}
