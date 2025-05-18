using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using bocchiwebbackend.Models;

namespace bocchiwebbackend.Repositories
{
    public interface IProductRepository : IRepository<Product>
    {
        Task<bool> ExistsByNameAsync(string name);
        Task<Product?> GetProductWithCategoryAsync(int id);
    }
}