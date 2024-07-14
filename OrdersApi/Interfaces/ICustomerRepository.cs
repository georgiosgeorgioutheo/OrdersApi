using OrdersApi.Models;

namespace OrdersApi.Interfaces
{
    
    public interface ICustomerRepository : IRepository<Customer>
    {

        Task<IEnumerable<Customer>> GetCustomersWithOrdersAsync();
    }
}
