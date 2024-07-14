using MediatR;
using OrdersApi.Interfaces;
using OrdersApi.Models;
using OrdersApi.Queries;

namespace OrdersApi.Handlers
{
    public class GetAllCustomersHandler : IRequestHandler<GetAllCustomersQuery, IEnumerable<Customer>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetAllCustomersHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Handles GetAll Customers Query
        /// </summary>
        /// <param name="request">The request</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>
        /// All customers
        /// </returns>
        public async Task<IEnumerable<Customer>> Handle(GetAllCustomersQuery request, CancellationToken cancellationToken)
        {
            try
            {

                return await _unitOfWork.Customers.GetAllAsync();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An unexpected error occurred while retrieving the customers.", ex);
            }

        }

    }
}
