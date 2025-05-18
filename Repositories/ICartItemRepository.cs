using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using bocchiwebbackend.Models;

namespace bocchiwebbackend.Repositories
{
    public interface ICartItemRepository : IRepository<CartItem>
    {
        Task<CartItem> GetCartItemByCartIdAndProductIdAsync(int cartId, int productId);
        Task<List<CartItem>> GetCartItemsByCartIdAsync(int cartId);
    }
}