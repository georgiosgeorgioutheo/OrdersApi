using MediatR;
using OrdersApi.Commands;
using OrdersApi.Interfaces;

namespace OrdersApi.Handlers
{
    public class DeleteOrderHandler : IRequestHandler<DeleteOrderCommand, Unit>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteOrderHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Handles Delete Order Command
        /// </summary>
        /// <param name="request">The request</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>
        /// Completed or not
        /// </returns>
        public async Task<Unit> Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var order = await _unitOfWork.Orders.GetByIdAsync(request.OrderId);

                if (order == null)
                {
                    // Handle order not found
                    return Unit.Value;
                }

                _unitOfWork.Orders.Remove(order);
                await _unitOfWork.CompleteAsync();

                return Unit.Value;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An unexpected error occurred while deleting the Order.", ex);
            }
        }
    }
}
