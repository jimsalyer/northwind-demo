using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NorthwindDemo.Models;
using NorthwindDemo.Repositories;
using Sieve.Models;
using Sieve.Services;

namespace NorthwindDemo.Controllers
{
    [ApiController]
    [Route("api/categories")]
    public class CategoriesController : ControllerBase
    {
        private readonly NorthwindContext _context;
        private readonly SieveProcessor _sieveProcessor;

        public CategoriesController(NorthwindContext context, SieveProcessor sieveProcessor)
        {
            _context = context;
            _sieveProcessor = sieveProcessor;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Category>> GetAll(string filters, string sorts)
        {
            var sieveModel = new SieveModel
            {
                Filters = filters,
                Sorts = !string.IsNullOrWhiteSpace(sorts) ? sorts : "categoryName"
            };

            var categories = _context.Categories?.AsNoTracking();
            var categoriesResult = _sieveProcessor.Apply(sieveModel, categories);

            return categoriesResult.ToList();
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<Category> GetById(int id)
        {
            var category = _context.Categories
                .AsNoTracking()
                .FirstOrDefault(c => c.CategoryId == id);

            if (category != null)
            {
                return category;
            }
            return NotFound();
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<Category> Create(Category category)
        {
            if (ModelState.IsValid)
            {
                category.CategoryId = _context.Categories.Any() ? _context.Categories.Max(c => c.CategoryId) + 1 : 1;

                _context.Categories.Add(category);
                _context.SaveChanges();

                return CreatedAtAction(nameof(GetById), new { id = category.CategoryId }, category);
            }
            return BadRequest();
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<Category> Update(int id, Category category)
        {
            var existingCategory = _context.Categories.Find(id);
            if (existingCategory != null)
            {
                if (ModelState.IsValid && category.CategoryId == id)
                {
                    existingCategory.CategoryName = category.CategoryName;
                    existingCategory.Description = category.Description;
                    existingCategory.Picture = category.Picture;

                    _context.Categories.Update(existingCategory);
                    _context.SaveChanges();

                    return existingCategory;
                }
                return BadRequest();
            }
            return NotFound();
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<Category> Delete(int id)
        {
            var category = _context.Categories.Find(id);
            if (category != null)
            {
                _context.Categories.Remove(category);
                _context.SaveChanges();

                return category;
            }
            return NotFound();
        }

        [HttpGet("{id}/products")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<IEnumerable<Product>> GetProducts(int id, string filters, string sorts)
        {
            var category = _context.Categories
                .AsNoTracking()
                .FirstOrDefault(c => c.CategoryId == id);

            if (category != null)
            {
                var sieveModel = new SieveModel
                {
                    Filters = filters,
                    Sorts = !string.IsNullOrWhiteSpace(sorts) ? sorts : "productName"
                };

                var products = _context.Products
                    .AsNoTracking()
                    .Where(p => p.CategoryId == id);

                var productsResult = _sieveProcessor.Apply(sieveModel, products);

                return productsResult.ToList();
            }
            return NotFound();
        }
    }
}
