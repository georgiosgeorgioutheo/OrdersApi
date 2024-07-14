using MediatR;
using OrdersApi.Commands;
using OrdersApi.Interfaces;
using OrdersApi.Models;

namespace OrdersApi.Handlers
{
    public class CreateCustomerHandler : IRequestHandler<CreateCustomerCommand, Customer>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateCustomerHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Handles Create Customer Command
        /// </summary>
        /// <param name="request">The request </param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>
        /// The new customer
        /// </returns>
        public async Task<Customer> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var customer = new Customer(request.FirstName, request.LastName, request.Address, request.PostalCode);
                await _unitOfWork.Customers.AddAsync(customer);
                await _unitOfWork.CompleteAsync();

                return customer;
            }
            catch (Exception ex)
            {

                throw new ApplicationException("An error occurred while adding the customer.", ex);
            }
        }
    }

}