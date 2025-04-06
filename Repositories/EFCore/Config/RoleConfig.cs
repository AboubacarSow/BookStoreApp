using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Repositories.EFCore.Config
{
    public class RoleConfig : IEntityTypeConfiguration<IdentityRole>
    {
        public void Configure(EntityTypeBuilder<IdentityRole> builder)
        {
            builder.HasData(
                new IdentityRole() 
                { 
                    Id= "d993f8c9-991d-4671-86f9-86b769a1c601",
                    Name="User",
                    NormalizedName="USER",
                    ConcurrencyStamp= "ecf1f447-76ba-4689-9abb-477549bf5b45"
                },
                new IdentityRole()
                { 
                    Id= "37a3a701 - 684f - 4de0 - a6b9 - 369b570b3ec4",
                    Name ="Editor",
                    NormalizedName="EDITOR",
                    ConcurrencyStamp = "899fc7d2-f5ab-49ef-90f7-cc7b32a76123"
                },
                new IdentityRole()
                {
                    Id= "064dfa1d-e00e-4383-bb12-2e65d35a4579",
                    Name = "Admin",
                    NormalizedName = "ADMIN",
                    ConcurrencyStamp = "d993f8c9-991d-4671-86f9-86b769a1c601"
                }
            );
        }
    }
}
