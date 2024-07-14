using MediatR;
using OrdersApi.Models;

namespace OrdersApi.Queries
{
    public class GetCustomerByIdQuery : IRequest<Customer>
    {
        public int CustomerId { get; set; }

        public GetCustomerByIdQuery(int customerId)
        {
            CustomerId = customerId;
        }
    }
}

