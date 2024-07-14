using MediatR;
using OrdersApi.Interfaces;
using OrdersApi.Models;
using OrdersApi.Queries;

namespace OrdersApi.Handlers
{
    public class GetCustomerByIdHandler : IRequestHandler<GetCustomerByIdQuery, Customer>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetCustomerByIdHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Handles Get CustomerById Query
        /// </summary>
        /// <param name="request">The request</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>
        /// The customer with the given id
        /// </returns>
        public async Task<Customer> Handle(GetCustomerByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                return await _unitOfWork.Customers.GetByIdAsync(request.CustomerId);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An unexpected error occurred while retrieving the customer by ID.", ex);
            }
        }
    }
}
