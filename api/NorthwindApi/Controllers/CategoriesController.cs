using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NorthwindApi.Models;
using NorthwindApi.Repositories;

namespace NorthwindApi.Controllers
{
    [ApiController]
    [Route("api/categories")]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoriesController(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Category>> List(string filters, string sorts)
        {
            return _categoryRepository.List(filters, sorts);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<Category> GetById(int id)
        {
            var category = _categoryRepository.GetById(id);
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
                var category = _categoryRepository.Add(categoryDto);
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
                var category = _categoryRepository.Update(id, categoryDto);
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
            var category = _categoryRepository.Delete(id);
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
            var products = _categoryRepository.ListProducts(id, filters, sorts);
            if (products != null)
            {
                return products;
            }
            return NotFound();
        }
    }
}
