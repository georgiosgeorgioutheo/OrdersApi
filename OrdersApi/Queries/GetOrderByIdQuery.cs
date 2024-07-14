using MediatR;
using OrdersApi.Models;

namespace OrdersApi.Queries
{
    public class GetOrderByIdQuery : IRequest<Order>
    {
        public int OrderId { get; set; }

        public GetOrderByIdQuery(int orderId)
        {
            OrderId = orderId;
        }
    }
}
