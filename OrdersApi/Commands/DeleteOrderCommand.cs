using MediatR;

namespace OrdersApi.Commands
{
    public class DeleteOrderCommand : IRequest<Unit>
    {
        public int OrderId { get; set; }

        public DeleteOrderCommand(int orderId)
        {
            OrderId = orderId;
        }
    }
}
