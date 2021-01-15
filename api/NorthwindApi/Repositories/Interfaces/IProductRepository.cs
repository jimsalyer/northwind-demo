using System.Collections.Generic;
using NorthwindApi.Models;

namespace NorthwindApi.Repositories
{
    public interface IProductRepository
    {
        List<Product> List(string filters, string sorts);
        Product GetById(int id);
        Product Add(ProductDto productDto);
        Product Update(int id, ProductDto productDto);
        Product Delete(int id);
    }
}
