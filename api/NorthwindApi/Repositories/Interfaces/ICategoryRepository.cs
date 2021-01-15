using System.Collections.Generic;
using NorthwindApi.Models;

namespace NorthwindApi.Repositories
{
    public interface ICategoryRepository
    {
        List<Category> List(string filters, string sorts);
        Category GetById(int id);
        Category Add(CategoryDto categoryDto);
        Category Update(int id, CategoryDto categoryDto);
        Category Delete(int id);
        List<Product> ListProducts(int categoryId, string filters, string sorts);
    }
}
