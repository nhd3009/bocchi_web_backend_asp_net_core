using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using bocchiwebbackend.Data;
using Microsoft.EntityFrameworkCore.Storage;

namespace bocchiwebbackend.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        private IDbContextTransaction _transaction;
        private bool _disposed = false;

        private IUserRepository _userRepository;
        private ICategoryRepository _categoryRepository;
        private IProductRepository _productRepository;
        private ICartRepository _cartRepository;
        private ICartItemRepository _cartItemRepository;
        private IOrderRepository _orderRepository;
        private IOrderItemRepository _orderItemRepository;


        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
        }

        public IUserRepository Users => _userRepository ??= new UserRepository(_context);
        public ICategoryRepository Categories => _categoryRepository ??= new CategoryRepository(_context);
        public IProductRepository Products => _productRepository ??= new ProductRepository(_context);
        public ICartRepository Carts => _cartRepository ??= new CartRepository(_context);
        public ICartItemRepository CartItems => _cartItemRepository ??= new CartItemRepository(_context);
        public IOrderRepository Orders => _orderRepository ??= new OrderRepository(_context);
        public IOrderItemRepository OrderItems => _orderItemRepository ??= new OrderItemRepository(_context);

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public async Task BeginTransactionAsync()
        {
            _transaction = await _context.Database.BeginTransactionAsync();
        }

        public async Task CommitTransactionAsync()
        {
            try
            {
                await _transaction?.CommitAsync();
            }
            finally
            {
                _transaction?.Dispose();
                _transaction = null;
            }
        }

        public async Task RollbackTransactionAsync()
        {
            try
            {
                await _transaction?.RollbackAsync();
            }
            finally
            {
                _transaction?.Dispose();
                _transaction = null;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _transaction?.Dispose();
                    _context.Dispose();
                }

                _disposed = true;
            }
        }
    }
}