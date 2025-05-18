using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using bocchiwebbackend.Data;
using bocchiwebbackend.Models;
using Microsoft.EntityFrameworkCore;

namespace bocchiwebbackend.Repositories
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        public ProductRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<bool> ExistsByNameAsync(string name)
        {
            return await _context.Products
                .AnyAsync(p => p.Name.ToLower() == name.ToLower());
        }

        public async Task<Product?> GetProductWithCategoryAsync(int id)
        {
            return await _context.Products
                .Include(p => p.Category)
                .FirstOrDefaultAsync(p => p.Id == id);
        }
    }
}