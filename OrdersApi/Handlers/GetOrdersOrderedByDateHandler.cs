using MediatR;
using OrdersApi.Interfaces;
using OrdersApi.Models;
using OrdersApi.Queries;

namespace OrdersApi.Handlers
{
    public class GetOrdersOrderedByDatehandler : IRequestHandler<GetOrdersOrderedByDateQuery, IEnumerable<Order>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetOrdersOrderedByDatehandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Handles GetOrdersOrderedByDate Query
        /// </summary>
        /// <param name="request">The request</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>
        /// All orders ordered by date
        /// </returns>
        public async Task<IEnumerable<Order>> Handle(GetOrdersOrderedByDateQuery request, CancellationToken cancellationToken)
        {
            try
            {
                return await _unitOfWork.Orders.GetOrdersSortedByDateAsync();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An unexpected error occurred while retrieving the orders sorted by date.", ex);
            }
        }
    }
}
