using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using NorthwindDemo.Models;
using NorthwindDemo.Repositories;

namespace NorthwindDemo.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CategoriesController : ControllerBase
    {
        private readonly NorthwindContext _context;

        public CategoriesController(NorthwindContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IEnumerable<Category> Get()
        {
            return _context.Categories;
        }
    }
}
