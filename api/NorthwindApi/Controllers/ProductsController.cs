using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NorthwindApi.Models;
using NorthwindApi.Repositories;

namespace NorthwindApi.Controllers
{
    [ApiController]
    [Route("api/products")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductRepository _productRepository;

        public ProductsController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Product>> List(string filters, string sorts)
        {
            return _productRepository.List(filters, sorts);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<Product> GetById(int id)
        {
            var product = _productRepository.GetById(id);
            if (product != null)
            {
                return product;
            }
            return NotFound();
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<Product> Create(ProductDto productDto)
        {
            if (ModelState.IsValid)
            {
                var product = _productRepository.Add(productDto);
                return CreatedAtAction(nameof(GetById), new { id = product.ProductId }, product);
            }
            return BadRequest();
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<Product> Update(int id, ProductDto productDto)
        {
            if (ModelState.IsValid)
            {
                var product = _productRepository.Update(id, productDto);
                if (product != null)
                {
                    return product;
                }
                return NotFound();
            }
            return BadRequest();
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<Product> Delete(int id)
        {
            var product = _productRepository.Delete(id);
            if (product != null)
            {
                return product;
            }
            return NotFound();
        }
    }
}
