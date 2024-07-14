using MediatR;
using OrdersApi.Interfaces;
using OrdersApi.Models;
using OrdersApi.Queries;

namespace OrdersApi.Handlers
{
    public class GetAllOrdersHandler : IRequestHandler<GetAllOrdersQuery, IEnumerable<Order>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetAllOrdersHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Handles Get AllOrders Query
        /// </summary>
        /// <param name="request">The request</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>
        /// All orders
        /// </returns>
        public async Task<IEnumerable<Order>> Handle(GetAllOrdersQuery request, CancellationToken cancellationToken)
        {
            try
            {
                return await _unitOfWork.Orders.GetAllAsync();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An unexpected error occurred while retrieving the orders.", ex);
            }
        }
    }
}
