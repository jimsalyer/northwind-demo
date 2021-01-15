using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NorthwindApi.Models;
using Sieve.Services;

namespace NorthwindApi.Repositories
{
    public class SupplierRepository : RepositoryBase, ISupplierRepository
    {
        public SupplierRepository(NorthwindContext context, IMapper mapper, ISieveProcessor sieveProcessor)
            : base(context, mapper, sieveProcessor)
        {
        }

        public List<Supplier> List(string filters, string sorts)
        {
            var sieveModel = CreateSieveModel(filters, sorts, "companyName");
            var suppliers = _context.Suppliers?.AsNoTracking();
            var suppliersResult = _sieveProcessor.Apply(sieveModel, suppliers);
            return suppliersResult.ToList();
        }

        public Supplier GetById(int id)
        {
            var supplier = _context.Suppliers
                .AsNoTracking()
                .FirstOrDefault(c => c.SupplierId == id);
            return supplier;
        }

        public Supplier Add(SupplierDto supplierDto)
        {
            var supplier = _mapper.Map<Supplier>(supplierDto);
            supplier.SupplierId = _context.Suppliers.Any() ? _context.Suppliers.Max(c => c.SupplierId) + 1 : 1;

            _context.Suppliers.Add(supplier);
            _context.SaveChanges();
            return supplier;
        }

        public Supplier Update(int id, SupplierDto supplierDto)
        {
            var supplier = _context.Suppliers.Find(id);
            if (supplier != null)
            {
                _mapper.Map(supplierDto, supplier);
                _context.Suppliers.Update(supplier);
                _context.SaveChanges();
            }
            return supplier;
        }

        public Supplier Delete(int id)
        {
            var supplier = _context.Suppliers.Find(id);
            if (supplier != null)
            {
                _context.Suppliers.Remove(supplier);
                _context.SaveChanges();
            }
            return supplier;
        }

        public List<Product> ListProducts(int supplierId, string filters, string sorts)
        {
            var supplier = _context.Suppliers
                .AsNoTracking()
                .FirstOrDefault(c => c.SupplierId == supplierId);

            if (supplier != null)
            {
                var products = _context.Products
                    .AsNoTracking()
                    .Where(p => p.SupplierId == supplierId);

                var sieveModel = CreateSieveModel(filters, sorts, "productName");
                var productsResult = _sieveProcessor.Apply(sieveModel, products);
                return productsResult.ToList();
            }
            return null;
        }
    }
}
