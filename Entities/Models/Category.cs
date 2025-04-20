namespace Entities.Models
{
    public class Category
    {
        public int CategoryId { get; set; }
        public string ? CategoryName { get; set; }
        public virtual ICollection<Book> Books { get; set; }
    }
}
