using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NorthwindApi.Models;
using NorthwindApi.Services;

namespace NorthwindApi.Controllers
{
    [ApiController]
    [Route("api/categories")]
    public class CategoriesController : ControllerBase
    {
        private readonly CategoryService _categoryService;

        public CategoriesController(CategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Category>> GetAll(string filters, string sorts)
        {
            return _categoryService.ListCategories(filters, sorts);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<Category> GetById(int id)
        {
            var category = _categoryService.GetCategory(id);
            if (category != null)
            {
                return category;
            }
            return NotFound();
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<Category> Create(CategoryDto categoryDto)
        {
            if (ModelState.IsValid)
            {
                var category = _categoryService.CreateCategory(categoryDto);
                return CreatedAtAction(nameof(GetById), new { id = category.CategoryId }, category);
            }
            return BadRequest();
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<Category> Update(int id, CategoryDto categoryDto)
        {
            if (ModelState.IsValid)
            {
                var category = _categoryService.UpdateCategory(id, categoryDto);
                if (category != null)
                {
                    return category;
                }
                return NotFound();
            }
            return BadRequest();
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<Category> Delete(int id)
        {
            var category = _categoryService.DeleteCategory(id);
            if (category != null)
            {
                return category;
            }
            return NotFound();
        }

        [HttpGet("{id}/products")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<IEnumerable<Product>> GetProducts(int id, string filters, string sorts)
        {
            var products = _categoryService.ListProducts(id, filters, sorts);
            if (products != null)
            {
                return products;
            }
            return NotFound();
        }
    }
}
