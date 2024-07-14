using OrdersApi.Interfaces;
using OrdersApi.Repositories;

namespace OrdersApi.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
            Customers = new CustomerRepository(context);
            Orders = new OrderRepository(context);
            Products = new ProductRepository(context);
        }

        public ICustomerRepository Customers { get; }
        public IOrderRepository Orders { get; }
        public IProductRepository Products { get; }
        public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
