using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CarPartsShop.Models;
using Domain;
using Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace CarPartsShop.Controllers
{
    public class ShopController : ControllerBase
    {
        private readonly ICategoryWriteRepository _categoryWriteRepository;
        private readonly ICategoryReadRepository _categoryReadRepository;

        public ShopController(ICategoryWriteRepository categoryWriteRepository, ICategoryReadRepository categoryReadRepository)
        {
            _categoryReadRepository = categoryReadRepository;
            _categoryWriteRepository = categoryWriteRepository;
        }

        [HttpPost("api/AddCategory")]
        public async Task<IActionResult> AddCategoryMakeModel([FromBody]CreateCategoryRequestModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var doesExist = await _categoryReadRepository.DoesCategoryExist(model.Name);

                if (doesExist && !model.ParentId.HasValue)
                {
                    return BadRequest("Category name already exists!");
                }

                Category category;
                Category returnModel;

                if (model.ParentId.HasValue)
                {
                    category = Category.CreateChildItem(model.Name, model.Description, model.ParentId.Value);
                    var currentCategory = await _categoryReadRepository.GetCategory(model.ParentId.Value);
                    currentCategory.AddChildCategory(category);
                    returnModel = _categoryWriteRepository.UpdateCategory(currentCategory);
                }
                else
                {
                    category = Category.CreateInitialItem(model.Name, model.Description);
                    returnModel = _categoryWriteRepository.AddCategory(category);
                }

                _categoryWriteRepository.SaveChanges();

                return Ok(returnModel);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /*[HttpPost("api/AddCategoryPart")]
        public IActionResult AddCategoryPart(CreateCategoryRequestModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (!model.ParentId.HasValue)
                {
                    return BadRequest("Parent category not found!");
                }

                var newCategory = Category.CreateChildItem(model.Name, model.Description, model.ParentId.Value,
                    CategoryType.Part);

                var categoryModel = _categoryWriteRepository.AddCategory(newCategory);
                _categoryWriteRepository.SaveChanges();

                return Ok(categoryModel);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }*/

        [HttpPost("api/AddCategoryItem")]
        public async Task<IActionResult> AddCategoryItem([FromBody] CreateCategoryItemRequestModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var category = await _categoryReadRepository.GetCategory(model.ParentCategoryId);

                if (category == null)
                {
                    return BadRequest("Category does not exist");
                }

                var item = Item.GetItem(model.ParentCategoryId, model.Name, model.Description,
                    model.Price);

                category.AddChildItem(item);

                var categoryModel = _categoryWriteRepository.UpdateCategory(category);
                _categoryWriteRepository.SaveChanges();

                return Ok(categoryModel);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("api/RemoveCategory/{categoryId}")]
        public async Task<IActionResult> DeleteCategoryItem(Guid categoryId)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var category = await _categoryReadRepository.GetCategory(categoryId);

                if (category == null)
                {
                    return BadRequest("Category was not found");
                }

                _categoryWriteRepository.RemoveCategory(category);
                _categoryWriteRepository.SaveChanges();

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
