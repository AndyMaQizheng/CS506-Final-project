using Microsoft.EntityFrameworkCore;
using store_api.Core.Models;

namespace store_api.Core.Contexts
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {
        }

        public DbSet<AppConfig> AppConfigSettings { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<ProductDto> ProductDtos { get; set; }
        public DbSet<AccessToken> AccessTokens { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Category> Categories { get; set; }
    }
}
