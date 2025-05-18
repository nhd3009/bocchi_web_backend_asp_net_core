using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using bocchiwebbackend.Data;
using bocchiwebbackend.Models;
using Microsoft.EntityFrameworkCore;

namespace bocchiwebbackend.Repositories
{
    public class CartItemRepository : Repository<CartItem>, ICartItemRepository
    {
        public CartItemRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<CartItem?> GetCartItemByCartIdAndProductIdAsync(int cartId, int productId)
        {
            return await _context.CartItems.FirstOrDefaultAsync(ci => ci.CartId == cartId && ci.ProductId == productId);
        }

        public async Task<List<CartItem>> GetCartItemsByCartIdAsync(int cartId)
        {
            return await _context.CartItems.Where(ci => ci.CartId == cartId).ToListAsync();
        }
    }
}