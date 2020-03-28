using System;
using System.Collections.Generic;
using System.Text;
using Domain;

namespace Infrastructure
{
    public interface ICategoryWriteRepository
    {
        Category AddCategory(Category category);
        Category UpdateCategory(Category category);
        Category RemoveCategory(Category category);
        void SaveChanges();
    }
    public class CategoryWriteRepository : ICategoryWriteRepository
    {

        private readonly ShopContext _context;

        public CategoryWriteRepository(ShopContext context)
        {
            _context = context;
        }

        public Category AddCategory(Category category)
        {
            return _context.Categories.Add(category).Entity;
        }
        public Category UpdateCategory(Category category)
        {
            return _context.Categories.Update(category).Entity;
        }

        public Category RemoveCategory(Category category)
        {
            return _context.Categories.Remove(category).Entity;
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}
