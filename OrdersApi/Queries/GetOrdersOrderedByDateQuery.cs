using MediatR;
using OrdersApi.Interfaces;
using OrdersApi.Models;

namespace OrdersApi.Queries
{
    public class GetOrdersOrderedByDateQuery : IRequest<IEnumerable<Order>> { }

}
