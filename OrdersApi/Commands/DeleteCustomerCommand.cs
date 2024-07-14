using MediatR;

namespace OrdersApi.Commands
{


    public class DeleteCustomerCommand : IRequest<Unit>
    {
        public int CustomerId { get; set; }

        public DeleteCustomerCommand(int customerId)
        {
            CustomerId = customerId;
        }
    }
}
