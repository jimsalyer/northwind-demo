using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NorthwindApi.Models;
using Sieve.Services;

namespace NorthwindApi.Repositories
{
    public class ProductRepository : RepositoryBase, IProductRepository
    {
        public ProductRepository(NorthwindContext context, IMapper mapper, ISieveProcessor sieveProcessor)
            : base(context, mapper, sieveProcessor)
        {
        }

        public List<Product> List(string filters, string sorts)
        {
            var sieveModel = CreateSieveModel(filters, sorts, "productName");
            var products = _context.Products?.AsNoTracking();
            var productsResult = _sieveProcessor.Apply(sieveModel, products);
            return productsResult.ToList();
        }

        public Product GetById(int id)
        {
            var product = _context.Products
                .AsNoTracking()
                .FirstOrDefault(c => c.ProductId == id);
            return product;
        }

        public Product Add(ProductDto productDto)
        {
            var product = _mapper.Map<Product>(productDto);
            product.ProductId = _context.Products.Any() ? _context.Products.Max(c => c.ProductId) + 1 : 1;

            _context.Products.Add(product);
            _context.SaveChanges();
            return product;
        }

        public Product Update(int id, ProductDto productDto)
        {
            var product = _context.Products.Find(id);
            if (product != null)
            {
                _mapper.Map(productDto, product);
                _context.Products.Update(product);
                _context.SaveChanges();
            }
            return product;
        }

        public Product Delete(int productId)
        {
            var product = _context.Products.Find(productId);
            if (product != null)
            {
                _context.Products.Remove(product);
                _context.SaveChanges();
            }
            return product;
        }
    }
}
