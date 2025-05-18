using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using bocchiwebbackend.Data;
using bocchiwebbackend.Models;
using Microsoft.EntityFrameworkCore;

namespace bocchiwebbackend.Repositories
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        public CategoryRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<bool> ExistsByNameAsync(string name)
        {
            return await _context.Categories
                .AnyAsync(c => c.Name.ToLower() == name.ToLower());
        }
    }
}