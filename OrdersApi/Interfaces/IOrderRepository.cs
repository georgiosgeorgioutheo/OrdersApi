using OrdersApi.Models;

namespace OrdersApi.Interfaces
{
   
    public interface IOrderRepository : IRepository<Order>
    {

        Task<IEnumerable<Order>> GetOrdersByCustomerIdAsync(int customerId);
        Task<IEnumerable<Order>> GetOrdersSortedByDateAsync();
    }
}
