using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using bocchiwebbackend.Models;

namespace bocchiwebbackend.Repositories
{
    public interface ICartRepository : IRepository<Cart>
    {
        Task<Cart> GetCartByUserIdAsync(int userId);
    }
}