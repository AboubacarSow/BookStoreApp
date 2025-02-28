using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace Repositories.EFCore.Config
{
    public class BookConfig : IEntityTypeConfiguration<Book>
    {
        public void Configure(EntityTypeBuilder<Book> builder)
        {
            builder.HasData(
                new Book { id = 1, Title = "Karagöz ve Hacivet", Price = 75 },
                new Book { id = 2, Title = "Mesnevi", Price = 175 },
                new Book { id = 3, Title = "Devlet", Price = 75 }
                );
        }
    }
}
