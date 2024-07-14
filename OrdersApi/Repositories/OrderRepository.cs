using Microsoft.EntityFrameworkCore;
using OrdersApi.Interfaces;
using OrdersApi.Models;

namespace OrdersApi.Repositories
{
    // Repositories/OrderRepository.cs
    public class OrderRepository : Repository<Order>, IOrderRepository
    {
        public OrderRepository(AppDbContext context) : base(context) { }

        public async Task<IEnumerable<Order>> GetOrdersByCustomerIdAsync(int customerId)
        {
            return await _context.Orders.Where(o => o.CustomerId == customerId).ToListAsync();
        }

        public async Task<IEnumerable<Order>> GetOrdersSortedByDateAsync()
        {
            return await _context.Orders
                .OrderBy(o => o.OrderDate)
                .ToListAsync();
        }
    }
}
