using MediatR;
using OrdersApi.Models;

namespace OrdersApi.Queries
{
  
   public class GetAllOrdersQuery : IRequest<IEnumerable<Order>> { }
    
}
