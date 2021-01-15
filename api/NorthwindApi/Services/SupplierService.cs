using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NorthwindApi.Models;
using NorthwindApi.Repositories;
using Sieve.Services;

namespace NorthwindApi.Services
{
    public class SupplierService : ServiceBase
    {
        public SupplierService(NorthwindContext context, IMapper mapper, SieveProcessor sieveProcessor)
            : base(context, mapper, sieveProcessor)
        {
        }

        public List<Supplier> ListSuppliers(string filters, string sorts)
        {
            var sieveModel = CreateSieveModel(filters, sorts, "companyName");
            var suppliers = _context.Suppliers?.AsNoTracking();
            var suppliersResult = _sieveProcessor.Apply(sieveModel, suppliers);
            return suppliersResult.ToList();
        }

        public Supplier GetSupplier(int supplierId)
        {
            var supplier = _context.Suppliers
                .AsNoTracking()
                .FirstOrDefault(c => c.SupplierId == supplierId);
            return supplier;
        }

        public Supplier CreateSupplier(SupplierDto supplierDto)
        {
            var supplier = _mapper.Map<Supplier>(supplierDto);
            supplier.SupplierId = _context.Suppliers.Any() ? _context.Suppliers.Max(c => c.SupplierId) + 1 : 1;

            _context.Suppliers.Add(supplier);
            _context.SaveChanges();
            return supplier;
        }

        public Supplier UpdateSupplier(int supplierId, SupplierDto supplierDto)
        {
            var supplier = _context.Suppliers.Find(supplierId);
            if (supplier != null)
            {
                _mapper.Map(supplierDto, supplier);
                _context.Suppliers.Update(supplier);
                _context.SaveChanges();
            }
            return supplier;
        }

        public Supplier DeleteSupplier(int supplierId)
        {
            var supplier = _context.Suppliers.Find(supplierId);
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
