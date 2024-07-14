using MediatR;
using OrdersApi.Models;

namespace OrdersApi.Queries
{
    public class GetAllCustomersQuery : IRequest<IEnumerable<Customer>> { }
}
