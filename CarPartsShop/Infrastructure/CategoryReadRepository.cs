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
        Task<List<Category>> GetAllCategoriesWithChildCategoriesAsync();
        Task<Category> GetCategory(Guid categoryId);
        Task<bool> DoesCategoryExist(string categoryName);
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

        public async Task<Category> GetCategory(Guid categoryId)
        {
            var category = await _context
                .Categories
                .Include(x => x.ChildCategories)
                .ThenInclude(x => x.ChildCategories)
                .ThenInclude(x => x.ChildCategories)
                .FirstOrDefaultAsync(x => x.CategoryId == categoryId);

            return category;
        }

        public async Task<List<Category>> GetAllCategoriesWithChildCategoriesAsync()
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

            /*var returnResult = new List<Category>();

            foreach (var category in categories)
            {
                if(categ)
            }*/

            return categories;
        }
    }
}
