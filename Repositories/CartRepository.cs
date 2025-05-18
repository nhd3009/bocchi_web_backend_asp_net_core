using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using bocchiwebbackend.Data;
using bocchiwebbackend.Models;
using Microsoft.EntityFrameworkCore;

namespace bocchiwebbackend.Repositories
{
    public class CartRepository : Repository<Cart>, ICartRepository
    {
        public CartRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<Cart?> GetCartByUserIdAsync(int userId)
        {
            return await _context.Carts
                .Include(c => c.CartItems)
                    .ThenInclude(ci => ci.Product)
                .FirstOrDefaultAsync(c => c.UserId == userId);
        }

    }
}