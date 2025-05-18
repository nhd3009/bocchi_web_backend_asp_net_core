using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using bocchiwebbackend.Models;

namespace bocchiwebbackend.Repositories
{
    public interface IOrderRepository : IRepository<Order>
    {
        Task<Order> GetOrderByIdAsync(int id);
        Task<List<Order>> GetOrdersByUserIdAsync(int userId);
        Task<bool> IsOrderCompletedAsync(int orderId);
    }
}