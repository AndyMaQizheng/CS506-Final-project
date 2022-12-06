using store_api.Core.Interfaces;
using store_api.Core.Repositories;

namespace store_api.Core.Contexts
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DatabaseContext _context;

        public UnitOfWork(DatabaseContext context)
        {
            _context = context;
        }

        public AppConfigRepository AppConfigRepository => new AppConfigRepository(_context);
        public CustomerRepository CustomerRepository => new CustomerRepository(_context);
        public ProductRepository ProductRepository => new ProductRepository(_context);
        public CartItemRepository CartItemRepository => new CartItemRepository(_context);
        public CartRepository CartRepository => new CartRepository(_context);
        public AccessTokenRepository AccessTokenRepository => new AccessTokenRepository(_context);
        public OrderItemRepository OrderItemRepository => new OrderItemRepository(_context);
        public OrderRepository OrderRepository => new OrderRepository(_context);


        public void Commit()
        {
            _context.SaveChanges();
        }

        public void RollBack()
        {
            _context.Dispose();
        }
    }
}
