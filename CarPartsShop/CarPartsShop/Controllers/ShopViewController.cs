using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Domain;
using Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarPartsShop.Controllers
{
    [Authorize(Roles = "Admin")]
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

        [AllowAnonymous]
        [HttpGet("api/GetCategoryNames")]
        public async Task<IActionResult> GetCategoryNames()
        {
            try
            {
                var categories = await _categoryReadRepository.GetCategoriesForSearch();

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

        [AllowAnonymous]
        [HttpGet("api/GetItemsForSearch/{categoryId}")]
        public async Task<IActionResult> GetItemsForSearch(Guid categoryId)
        {
            try
            {
                var category = await _categoryReadRepository.GetCategoriesWithItems(categoryId);

                if (category == null)
                {
                    return BadRequest("Category not found");
                }

                var list = new List<Category>
                {
                    category
                };

                var categoryChildItems = GetCategoryItem(list, new List<Item>());


                return Ok(categoryChildItems);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
      
        [AllowAnonymous]
        [HttpGet("api/GetItems/{categoryId}/{itemId}")]
        public async Task<IActionResult> GetSingleItem(Guid categoryId, Guid itemId)
        {
            var current = User;

            try
            {
                var category = await _categoryReadRepository.GetCategoryWithItems(categoryId);

                if (category == null)
                {
                    return BadRequest("Category not found");
                }

                var item = category.ChildItems.FirstOrDefault(x => x.ItemId == itemId);

                if (item == null)
                {
                    return BadRequest("Item was not found");
                }

                return Ok(item);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [AllowAnonymous]
        [HttpGet("api/GetAllItems")]
        public async Task<IActionResult> GetAllItems()
        {
            try
            {

                var itemsList = new List<Item>();
                var categories = await _categoryReadRepository.GetAllCategoriesWithChildCategoriesAndItemsAsync();

                var newList = GetCategoryItem(categories, itemsList);
                
                return Ok(newList);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        private List<Item> GetCategoryItem(List<Category> categories, List<Item> items)
        {
            foreach (var category in categories)
            {
                if (category.ChildItems.Any())
                {
                    items.AddRange(category.ChildItems);
                }
                if(category.ChildCategories.Any())
                {
                    GetCategoryItem(category.ChildCategories.ToList(), items);
                }
            }

            return items;
        }
        private async Task<List<Item>> GetParentItems(Category category, List<Item> list)
        {
            if (!category.ParentCategoryId.HasValue) return list;

            var parentCategory = await _categoryReadRepository.GetCategoryWithItems(category.ParentCategoryId.Value);
            list.AddRange(parentCategory.ChildItems);
            await GetParentItems(parentCategory, list);

            return list;

        }
    }
}
