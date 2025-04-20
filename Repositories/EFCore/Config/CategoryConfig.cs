using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Repositories.EFCore.Config
{
    public class CategoryConfig : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.HasData(
                new Category { CategoryId=1, CategoryName="Tarih"},
                new Category { CategoryId=2, CategoryName="Edebiyat"},
                new Category { CategoryId=3, CategoryName="Coğrafiya"},
                new Category { CategoryId=4, CategoryName="Bilim"},
                new Category { CategoryId=5, CategoryName="Şiir"},
                new Category { CategoryId=6, CategoryName="Felsefe"}
                
                );
        }
    }
}
