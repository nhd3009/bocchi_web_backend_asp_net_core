using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using bocchiwebbackend.Data;
using bocchiwebbackend.Models;
using Microsoft.EntityFrameworkCore;

namespace bocchiwebbackend.Repositories
{
    public class OrderRepository : Repository<Order>, IOrderRepository
    {
        public OrderRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<Order> GetOrderByIdAsync(int id)
        {
            return await _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .FirstOrDefaultAsync(o => o.Id == id);
        }

        public async Task<List<Order>> GetOrdersByUserIdAsync(int userId)
        {
            return await _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .Where(o => o.UserId == userId)
                .ToListAsync();
        }

        public async Task<bool> IsOrderCompletedAsync(int orderId)
        {
            return await _context.Orders
                .AnyAsync(o => o.Id == orderId && o.Status == "Completed");
        }
    }
}