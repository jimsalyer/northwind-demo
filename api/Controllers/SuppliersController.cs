using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NorthwindApi.Models;
using NorthwindApi.Repositories;
using Sieve.Models;
using Sieve.Services;

namespace NorthwindApi.Controllers
{
    [ApiController]
    [Route("api/suppliers")]
    public class SuppliersController : ControllerBase
    {
        private readonly NorthwindContext _context;
        private readonly ISieveProcessor _sieveProcessor;

        public SuppliersController(NorthwindContext context, ISieveProcessor sieveProcessor)
        {
            _context = context;
            _sieveProcessor = sieveProcessor;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Supplier>> GetAll(string filters, string sorts)
        {
            var sieveModel = new SieveModel
            {
                Filters = filters,
                Sorts = !string.IsNullOrWhiteSpace(sorts) ? sorts : "companyName"
            };

            var suppliers = _context.Suppliers?.AsNoTracking();
            var suppliersResult = _sieveProcessor.Apply(sieveModel, suppliers);

            return suppliersResult.ToList();
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<Supplier> GetById(int id)
        {
            var supplier = _context.Suppliers
                .AsNoTracking()
                .FirstOrDefault(s => s.SupplierId == id);

            if (supplier != null)
            {
                return supplier;
            }
            return NotFound();
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<Category> Create(Supplier supplier)
        {
            if (ModelState.IsValid)
            {
                supplier.SupplierId = _context.Suppliers.Any() ? _context.Suppliers.Max(c => c.SupplierId) + 1 : 1;

                _context.Suppliers.Add(supplier);
                _context.SaveChanges();

                return CreatedAtAction(nameof(GetById), new { id = supplier.SupplierId }, supplier);
            }
            return BadRequest();
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<Supplier> Update(int id, Supplier supplier)
        {
            var existingSupplier = _context.Suppliers.Find(id);
            if (existingSupplier != null)
            {
                if (ModelState.IsValid && supplier.SupplierId == id)
                {
                    existingSupplier.CompanyName = supplier.CompanyName;
                    existingSupplier.ContactName = supplier.ContactName;
                    existingSupplier.ContactTitle = supplier.ContactTitle;
                    existingSupplier.Address = supplier.Address;
                    existingSupplier.City = supplier.City;
                    existingSupplier.Region = supplier.Region;
                    existingSupplier.PostalCode = supplier.PostalCode;
                    existingSupplier.Country = supplier.Country;
                    existingSupplier.Phone = supplier.Phone;
                    existingSupplier.Fax = supplier.Fax;
                    existingSupplier.Homepage = supplier.Homepage;

                    _context.Suppliers.Update(existingSupplier);
                    _context.SaveChanges();

                    return existingSupplier;
                }
                return BadRequest();
            }
            return NotFound();
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<Supplier> Delete(int id)
        {
            var supplier = _context.Suppliers.Find(id);
            if (supplier != null)
            {
                _context.Suppliers.Remove(supplier);
                _context.SaveChanges();

                return supplier;
            }
            return NotFound();
        }

        [HttpGet("{id}/products")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<IEnumerable<Product>> GetProducts(int id, string filters, string sorts)
        {
            var supplier = _context.Suppliers
                .AsNoTracking()
                .FirstOrDefault(c => c.SupplierId == id);

            if (supplier != null)
            {
                var sieveModel = new SieveModel
                {
                    Filters = filters,
                    Sorts = !string.IsNullOrWhiteSpace(sorts) ? sorts : "productName"
                };

                var products = _context.Products
                    .AsNoTracking()
                    .Where(p => p.SupplierId == id);

                var productsResult = _sieveProcessor.Apply(sieveModel, products);

                return productsResult.ToList();
            }
            return NotFound();
        }
    }
}
