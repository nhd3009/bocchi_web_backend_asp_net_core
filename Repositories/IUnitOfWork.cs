using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace bocchiwebbackend.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        IUserRepository Users { get; }
        ICategoryRepository Categories { get; }
        IProductRepository Products { get; }
        ICartRepository Carts { get; }
        ICartItemRepository CartItems { get; }
        IOrderRepository Orders { get; }
        IOrderItemRepository OrderItems { get; }

        Task<int> SaveChangesAsync();

        Task BeginTransactionAsync();
        Task CommitTransactionAsync();
        Task RollbackTransactionAsync();
    }
}