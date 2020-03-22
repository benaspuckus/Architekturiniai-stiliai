using System;
using System.Collections.Generic;
using System.Text;
using Domain;
using Infrastructure.EntityTypeConfiguration;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure
{
    public class ShopContext : DbContext
    {
        public DbSet<Category> Categories { get; set; }

        public ShopContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfiguration(new CategoryEntityTypeConfiguration());
            builder.ApplyConfiguration(new ItemEntityTypeConfiguration());
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
            }
        }
    }
}
