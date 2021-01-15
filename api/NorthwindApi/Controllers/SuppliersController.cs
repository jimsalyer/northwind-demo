using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NorthwindApi.Models;
using NorthwindApi.Repositories;

namespace NorthwindApi.Controllers
{
    [ApiController]
    [Route("api/suppliers")]
    public class SuppliersController : ControllerBase
    {
        private readonly ISupplierRepository _supplierRepository;

        public SuppliersController(ISupplierRepository supplierRepository)
        {
            _supplierRepository = supplierRepository;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Supplier>> List(string filters, string sorts)
        {
            return _supplierRepository.List(filters, sorts);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<Supplier> GetById(int id)
        {
            var supplier = _supplierRepository.GetById(id);
            if (supplier != null)
            {
                return supplier;
            }
            return NotFound();
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<Category> Create(SupplierDto supplierDto)
        {
            if (ModelState.IsValid)
            {
                var supplier = _supplierRepository.Add(supplierDto);
                return CreatedAtAction(nameof(GetById), new { id = supplier.SupplierId }, supplier);
            }
            return BadRequest();
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<Supplier> Update(int id, SupplierDto supplierDto)
        {
            if (ModelState.IsValid)
            {
                var supplier = _supplierRepository.Update(id, supplierDto);
                if (supplier != null)
                {
                    return supplier;
                }
                return NotFound();
            }
            return BadRequest();
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<Supplier> Delete(int id)
        {
            var supplier = _supplierRepository.Delete(id);
            if (supplier != null)
            {
                return supplier;
            }
            return NotFound();
        }

        [HttpGet("{id}/products")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<IEnumerable<Product>> GetProducts(int id, string filters, string sorts)
        {
            var products = _supplierRepository.ListProducts(id, filters, sorts);
            if (products != null)
            {
                return products;
            }
            return NotFound();
        }
    }
}
