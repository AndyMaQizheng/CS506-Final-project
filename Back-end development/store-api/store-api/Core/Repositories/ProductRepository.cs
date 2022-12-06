using Microsoft.EntityFrameworkCore;
using store_api.Core.Contexts;
using store_api.Core.Models;
using System.Collections.Generic;
using System.Linq;

namespace store_api.Core.Repositories
{
    public class ProductRepository : BaseRepository<Product>
    {
        public ProductRepository(DatabaseContext context) : base(context)
        {

        }

        public List<ProductDto> GetProducts(string productname)
        {
            return _context.ProductDtos.FromSqlRaw("select Name, Price, (select ImagePath from productimages where productid=id)Image from products where Name like {1} ", new object[] { "%" + productname + "%" }).ToList();
        }

    }
}
