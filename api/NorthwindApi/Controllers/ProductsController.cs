using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NorthwindApi.Models;
using NorthwindApi.Services;

namespace NorthwindApi.Controllers
{
    [ApiController]
    [Route("api/products")]
    public class ProductsController : ControllerBase
    {
        private readonly ProductService _productService;

        public ProductsController(ProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Product>> GetAll(string filters, string sorts)
        {
            return _productService.ListProducts(filters, sorts);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<Product> GetById(int id)
        {
            var product = _productService.GetProduct(id);
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
                var product = _productService.CreateProduct(productDto);
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
                var product = _productService.UpdateProduct(id, productDto);
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
            var product = _productService.DeleteProduct(id);
            if (product != null)
            {
                return product;
            }
            return NotFound();
        }
    }
}
