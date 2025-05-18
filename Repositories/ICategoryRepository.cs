using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using bocchiwebbackend.Models;

namespace bocchiwebbackend.Repositories
{
    public interface ICategoryRepository : IRepository<Category>
    {
        Task<bool> ExistsByNameAsync(string name);
    }
}