using MediatR;
using OrdersApi.Models;

namespace OrdersApi.Commands
{
   
    public class CreateOrderCommand : IRequest<Order>
    {
       
        public int CustomerId { get; set; }
        public DateTime OrderDate { get; set; }
        public List<OrderItemDto> Items { get; set; }

       
        public class OrderItemDto
        {
            public int ProductId { get; set; }
            public int Quantity { get; set; }
            public decimal Price { get; set; }
        }

    }
}
