using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NorthwindApi.Models;
using NorthwindApi.Repositories;
using Sieve.Services;

namespace NorthwindApi.Services
{
    public class ProductService : ServiceBase
    {
        public ProductService(NorthwindContext context, IMapper mapper, SieveProcessor sieveProcessor)
            : base(context, mapper, sieveProcessor)
        {
        }

        public List<Product> ListProducts(string filters, string sorts)
        {
            var sieveModel = CreateSieveModel(filters, sorts, "productName");
            var products = _context.Products?.AsNoTracking();
            var productsResult = _sieveProcessor.Apply(sieveModel, products);
            return productsResult.ToList();
        }

        public Product GetProduct(int productId)
        {
            var product = _context.Products
                .AsNoTracking()
                .FirstOrDefault(c => c.ProductId == productId);
            return product;
        }

        public Product CreateProduct(ProductDto productDto)
        {
            var product = _mapper.Map<Product>(productDto);
            product.ProductId = _context.Products.Any() ? _context.Products.Max(c => c.ProductId) + 1 : 1;

            _context.Products.Add(product);
            _context.SaveChanges();
            return product;
        }

        public Product UpdateProduct(int productId, ProductDto productDto)
        {
            var product = _context.Products.Find(productId);
            if (product != null)
            {
                _mapper.Map(productDto, product);
                _context.Products.Update(product);
                _context.SaveChanges();
            }
            return product;
        }

        public Product DeleteProduct(int productId)
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
