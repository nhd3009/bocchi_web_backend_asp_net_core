using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using bocchiwebbackend.Models;

namespace bocchiwebbackend.Repositories
{
    public interface IOrderItemRepository : IRepository<OrderItem>
    {
        Task<List<OrderItem>> GetOrderItemsByOrderIdAsync(int orderId);
    }
}