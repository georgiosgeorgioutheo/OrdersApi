using Microsoft.EntityFrameworkCore;
using OrdersApi.Interfaces;
using OrdersApi.Models;

namespace OrdersApi.Repositories
{
    
    public class CustomerRepository : Repository<Customer>, ICustomerRepository
    {
        public CustomerRepository(AppDbContext context) : base(context) { }

        public async Task<IEnumerable<Customer>> GetCustomersWithOrdersAsync()
        {
            return await _context.Customers.Include(c => c.Orders).ToListAsync();
        }
    }
}
