using System;
using System.Collections.Generic;
using System.Text;
using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.EntityTypeConfiguration
{
    public class ShoppingCartEntityTypeConfiguration : IEntityTypeConfiguration<ShoppingCart>
    {
        public void Configure(EntityTypeBuilder<ShoppingCart> builder)
        {
            builder.ToTable(nameof(ShoppingCart));
            builder.HasKey(x => x.CartId);
            builder.Property(x => x.Status).IsRequired();
            builder.Property(x => x.NeedsDelivery).IsRequired();
            builder.Property(x => x.UserId).IsRequired();
        }
    }
}