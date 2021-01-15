using System.Collections.Generic;
using NorthwindApi.Models;

namespace NorthwindApi.Repositories
{
    public interface ISupplierRepository
    {
        List<Supplier> List(string filters, string sorts);
        Supplier GetById(int id);
        Supplier Add(SupplierDto supplierDto);
        Supplier Update(int id, SupplierDto supplierDto);
        Supplier Delete(int id);
        List<Product> ListProducts(int supplierId, string filters, string sorts);
    }
}
