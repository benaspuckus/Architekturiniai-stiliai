using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure
{
    public interface ICategoryReadRepository
    {
        Task<List<Category>> GetAllCategoriesWithChildCategoriesAndItemsAsync();
        Task<Category> GetCategoryWithChildren(Guid categoryId);
        Task<bool> DoesCategoryExist(string categoryName);
        Task<Category> GetCategoryWithItems(Guid categoryId);
        Task<List<Category>> GetCategoriesForSearch();
        Task<Category> GetCategoriesWithItems(Guid categoryId);
    }
    public class CategoryReadRepository : ICategoryReadRepository
    {
        private readonly ShopContext _context;

        public CategoryReadRepository(ShopContext context)
        {
            _context = context;
        }

        public async Task<bool> DoesCategoryExist(string categoryName)
        {
            var category = await _context
                .Categories
                .FirstOrDefaultAsync(x => x.Name == categoryName);

            return category != null;
        }

        public async Task<Category> GetCategoryWithChildren(Guid categoryId)
        {
            var category = await _context
                .Categories
                .Include(x => x.ChildCategories)
                .ThenInclude(x => x.ChildCategories)
                .ThenInclude(x => x.ChildCategories)
                .FirstOrDefaultAsync(x => x.CategoryId == categoryId);

            return category;
        }

        public async Task<List<Category>> GetCategoriesForSearch()
        {
            var category = await _context
                .Categories
                .Include(x => x.ChildCategories)
                .ThenInclude(x => x.ChildCategories)
                .ThenInclude(x => x.ChildCategories)
                .Where(x => !x.ParentCategoryId.HasValue)
                .AsNoTracking()
                .ToListAsync();

            return category;
        }

        public async Task<Category> GetCategoryWithItems(Guid categoryId)
        {
            var category = await _context
                .Categories
                .Include(x => x.ChildItems)
                .FirstOrDefaultAsync(x => x.CategoryId == categoryId);

            return category;
        }

        public async Task<Category> GetCategoriesWithItems(Guid categoryId)
        {
            var category = await _context
                .Categories
                .Include(x => x.ChildCategories)
                .ThenInclude(x => x.ChildCategories)
                .ThenInclude(x => x.ChildCategories)
                .ThenInclude(x => x.ChildItems)
                .Include(x => x.ChildCategories)
                .ThenInclude(x => x.ChildCategories)
                .ThenInclude(x => x.ChildItems)
                .Include(x => x.ChildCategories)
                .ThenInclude(x => x.ChildItems)
                .Include(x => x.ChildItems)
                .FirstOrDefaultAsync(x => x.CategoryId == categoryId);

            return category;
        }

        public async Task<List<Category>> GetAllCategoriesWithChildCategoriesAndItemsAsync()
        {
            var categories = await _context
                .Categories
                .Include(x => x.ChildCategories)
                    .ThenInclude(x => x.ChildCategories)
                        .ThenInclude(x => x.ChildCategories)
                            .ThenInclude(x => x.ChildCategories)
                                .ThenInclude(x => x.ChildCategories)
                                    .ThenInclude(x => x.ChildItems)
                .Include(x => x.ChildCategories)
                    .ThenInclude(x => x.ChildCategories)
                        .ThenInclude(x => x.ChildCategories)
                            .ThenInclude(x => x.ChildCategories)
                                .ThenInclude(x => x.ChildItems)
                .Include(x => x.ChildCategories)
                    .ThenInclude(x => x.ChildCategories)
                        .ThenInclude(x => x.ChildCategories)
                                .ThenInclude(x => x.ChildItems)
                .Include(x => x.ChildCategories)
                    .ThenInclude(x => x.ChildCategories)
                        .ThenInclude(x => x.ChildItems)
                .Include(x => x.ChildCategories)
                    .ThenInclude(x => x.ChildItems)
                .Include(x => x.ChildItems)
                .Where(x => !x.ParentCategoryId.HasValue)
                .AsNoTracking()
                .ToListAsync();

            return categories;
        }
    }
}
