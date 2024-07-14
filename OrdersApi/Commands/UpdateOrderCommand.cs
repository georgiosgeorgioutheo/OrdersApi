using MediatR;
using OrdersApi.Models;

namespace OrdersApi.Commands
{
    public class UpdateOrderCommand : IRequest<Order>
    {
        public int OrderId { get; set; }

        public int CustomerId { get; set; }
        public DateTime OrderDate { get; set; }
        public List<OrderItemUpdateDto> Items { get; set; }

        public class OrderItemUpdateDto
        {
            public int ProductId { get; set; }
            public int Quantity { get; set; }
            public decimal Price { get; set; }
        }
    }
}
