using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using bocchiwebbackend.Data;
using bocchiwebbackend.Models;
using Microsoft.EntityFrameworkCore;

namespace bocchiwebbackend.Repositories
{
    public class OrderItemRepository : Repository<OrderItem>, IOrderItemRepository
    {
        public OrderItemRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<List<OrderItem>> GetOrderItemsByOrderIdAsync(int orderId)
        {
            return await _context.OrderItems
                .Include(oi => oi.Product)
                .Where(oi => oi.OrderId == orderId)
                .ToListAsync();
        }
    }
}