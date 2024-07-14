using MediatR;
using OrdersApi.Commands;
using OrdersApi.Interfaces;
using System.Threading;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;
namespace OrdersApi.Handlers
{
 

    public class DeleteCustomerHandler : IRequestHandler<DeleteCustomerCommand, Unit>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteCustomerHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Handles Delete Customer Command
        /// </summary>
        /// <param name="request">The request</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>
        /// Completed or not
        /// </returns>
        public async Task<Unit> Handle(DeleteCustomerCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var customer = await _unitOfWork.Customers.GetByIdAsync(request.CustomerId);

                if (customer == null)
                {
                    // Handle customer not found
                    return Unit.Value;
                }

                _unitOfWork.Customers.Remove(customer);
                await _unitOfWork.CompleteAsync();

                return Unit.Value;
            }
            catch (Exception ex)
            {

                throw new ApplicationException("An unexpected error occurred while deleting the customer.", ex);
            }

        }

        }
}
