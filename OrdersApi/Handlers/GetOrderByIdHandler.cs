using MediatR;
using OrdersApi.Interfaces;
using OrdersApi.Models;
using OrdersApi.Queries;

namespace OrdersApi.Handlers
{
    public class GetOrderByIdHandler : IRequestHandler<GetOrderByIdQuery, Order>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetOrderByIdHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Order> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                return await _unitOfWork.Orders.GetByIdAsync(request.OrderId);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An unexpected error occurred while retrieving the order by ID.", ex);
            }
        }
    }

}
