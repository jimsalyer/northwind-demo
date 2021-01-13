using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NorthwindDemo.Models;
using NorthwindDemo.Repositories;

namespace NorthwindDemo.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriesController : ControllerBase
    {
        private readonly NorthwindContext _context;

        public CategoriesController(NorthwindContext context)
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Category>> GetAll()
        {
            return _context.Categories;
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<Category> GetById(int id)
        {
            Category category = _context.Categories.Find(id);
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
            if (category != null &&
                category.CategoryId > 0 &&
                !string.IsNullOrWhiteSpace(category.CategoryName) &&
                !_context.Categories.Any(c => c.CategoryId == category.CategoryId))
            {
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
            Category existingCategory = _context.Categories.Find(id);
            if (existingCategory != null)
            {
                if (category != null &&
                    category.CategoryId == id &&
                    !string.IsNullOrWhiteSpace(category.CategoryName))
                {
                    existingCategory.CategoryName = category.CategoryName;
                    existingCategory.Description = category.Description;

                    _context.Categories.Update(existingCategory);
                    _context.SaveChanges();
                    return category;
                }
                return BadRequest();
            }
            return NotFound();
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<Category> Delete(int id)
        {
            Category category = _context.Categories.Find(id);
            if (category != null)
            {
                _context.Categories.Remove(category);
                _context.SaveChanges();
                return category;
            }
            return NotFound();
        }
    }
}
