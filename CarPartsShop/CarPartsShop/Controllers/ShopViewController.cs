using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace CarPartsShop.Controllers
{
    public class ShopViewController : ControllerBase
    {
        private readonly ICategoryReadRepository _categoryReadRepository;

        public ShopViewController(ICategoryReadRepository categoryReadRepository)
        {
            _categoryReadRepository = categoryReadRepository;
        }

        [HttpGet("api/GetCategories")]
        public async Task<IActionResult> GetAllCategories()
        {
            try
            {
                var categories = await _categoryReadRepository.GetAllCategoriesWithChildCategoriesAndItemsAsync();

                return Ok(categories);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("api/GetItems/{categoryId}")]
        public async Task<IActionResult> GetAllCategories(Guid categoryId)
        {
            try
            {
                var category = await _categoryReadRepository.GetCategoryWithItems(categoryId);

                if (category == null)
                {
                    return BadRequest("Category not found");
                }

                return Ok(category.ChildItems);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
