namespace Entities.Models
{
    public class Book
    {
        public int id { get; set; }
        public string Title { get; set; }   
        public decimal Price { get; set; }
        public int CategoryId { get; set; }
        public virtual Category Category { get; set; }
    }
}
