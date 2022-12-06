using store_api.Core.Repositories;

namespace store_api.Core.Interfaces
{
    public interface IUnitOfWork
    {
        void Commit();
        void RollBack();
        AppConfigRepository AppConfigRepository { get; }
        CustomerRepository CustomerRepository { get; }
        CartRepository CartRepository { get; }
        ProductRepository ProductRepository { get; }
        CartItemRepository CartItemRepository { get; }
        AccessTokenRepository AccessTokenRepository { get; }
        OrderRepository OrderRepository { get; }
        OrderItemRepository OrderItemRepository { get; }
    }
}
