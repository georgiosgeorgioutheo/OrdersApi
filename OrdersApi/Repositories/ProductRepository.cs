using OrdersApi.Interfaces;
using OrdersApi.Models;

namespace OrdersApi.Repositories
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        public ProductRepository(AppDbContext context) : base(context) { }
    }
}
