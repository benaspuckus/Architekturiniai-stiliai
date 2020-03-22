

using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.EntityTypeConfiguration
{
    public class CategoryEntityTypeConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.ToTable(nameof(Category));
            builder.HasKey(x => x.CategoryId);
            builder.Property(x => x.Name).IsRequired();
            builder.Property(x => x.Description);
            builder.HasMany(x => x.ChildCategories).WithOne().HasForeignKey(x => x.ParentCategoryId);
            builder.HasMany(x => x.ChildItems).WithOne().HasForeignKey(x => x.ParentCategoryId);
        }
    }
}
