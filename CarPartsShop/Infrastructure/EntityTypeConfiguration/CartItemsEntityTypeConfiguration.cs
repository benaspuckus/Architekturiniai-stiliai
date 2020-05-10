using System;
using System.Collections.Generic;
using System.Text;
using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.EntityTypeConfiguration
{
    public class CartItemsEntityTypeConfiguration : IEntityTypeConfiguration<CartItems>
    {
        public void Configure(EntityTypeBuilder<CartItems> builder)
        {
            builder.ToTable(nameof(CartItems));
            builder.HasKey(x => new {x.CartId, x.ItemId});
            builder.HasOne<Item>(x => x.Item).WithMany(x => x.CartItems).HasForeignKey(x => x.ItemId);
            builder.HasOne<ShoppingCart>(x => x.ShoppingCart).WithMany(x => x.CartItems).HasForeignKey(x => x.CartId);
        }
    }
}
