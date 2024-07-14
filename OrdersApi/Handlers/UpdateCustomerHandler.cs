namespace OrdersApi.Handlers
{
    using MediatR;
    using OrdersApi.Commands;
    using OrdersApi.Interfaces;
    using OrdersApi.Models;
    using System.Threading;
    using System.Threading.Tasks;

    public class UpdateCustomerHandler : IRequestHandler<UpdateCustomerCommand, Customer>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateCustomerHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Handles Update Customer Command
        /// </summary>
        /// <param name="request">The request</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>
        /// The updated customer
        /// </returns>
        public async Task<Customer> Handle(UpdateCustomerCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var customer = await _unitOfWork.Customers.GetByIdAsync(request.CustomerId);

                if (customer == null)
                {

                    throw new KeyNotFoundException($"Customer with ID {request.CustomerId} was not found.");
                }

                customer = new Customer(request.FirstName, request.LastName, request.Address, request.PostalCode); // Updating the customer details
                _unitOfWork.Customers.Update(customer);
                await _unitOfWork.CompleteAsync();

                return customer;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An unexpected error occurred while updating the customer.", ex);
            }
        }
    }
}
