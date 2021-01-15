using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NorthwindApi.Models;
using Sieve.Services;

namespace NorthwindApi.Repositories
{
    public class CategoryRepository : RepositoryBase, ICategoryRepository
    {
        public CategoryRepository(NorthwindContext context, IMapper mapper, ISieveProcessor sieveProcessor)
            : base(context, mapper, sieveProcessor)
        {
        }

        public List<Category> List(string filters, string sorts)
        {
            var sieveModel = CreateSieveModel(filters, sorts, "categoryName");
            var categories = _context.Categories.AsNoTracking();
            var categoriesResult = _sieveProcessor.Apply(sieveModel, categories);
            return categoriesResult.ToList();
        }

        public Category GetById(int id)
        {
            var category = _context.Categories
                .AsNoTracking()
                .FirstOrDefault(c => c.CategoryId == id);
            return category;
        }

        public Category Add(CategoryDto categoryDto)
        {
            var category = _mapper.Map<Category>(categoryDto);
            category.CategoryId = _context.Categories.Any() ? _context.Categories.Max(c => c.CategoryId) + 1 : 1;

            _context.Categories.Add(category);
            _context.SaveChanges();
            return category;
        }

        public Category Update(int id, CategoryDto categoryDto)
        {
            var category = _context.Categories.Find(id);
            if (category != null)
            {
                _mapper.Map(categoryDto, category);
                _context.Categories.Update(category);
                _context.SaveChanges();
            }
            return category;
        }

        public Category Delete(int id)
        {
            var category = _context.Categories.Find(id);
            if (category != null)
            {
                _context.Categories.Remove(category);
                _context.SaveChanges();
            }
            return category;
        }

        public List<Product> ListProducts(int categoryId, string filters, string sorts)
        {
            var category = _context.Categories
                .AsNoTracking()
                .FirstOrDefault(c => c.CategoryId == categoryId);

            if (category != null)
            {
                var products = _context.Products
                    .AsNoTracking()
                    .Where(p => p.CategoryId == categoryId);

                var sieveModel = CreateSieveModel(filters, sorts, "productName");
                var productsResult = _sieveProcessor.Apply(sieveModel, products);
                return productsResult.ToList();
            }
            return null;
        }
    }
}
