using MediatR;
using OrdersApi.Commands;
using OrdersApi.Interfaces;
using OrdersApi.Models;

namespace OrdersApi.Handlers
{

    public class AddProductHandler : IRequestHandler<AddProductCommand, Product>
    {
        private readonly IUnitOfWork _unitOfWork;

        public AddProductHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Handles  Add Product 
        /// </summary>
        /// <param name="request">The request</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>
        /// new Product
        /// </returns>
        public async Task<Product> Handle(AddProductCommand request, CancellationToken cancellationToken)
        {


            try
            {
                var product = new Product
                {
                    Name = request.Name,
                    Price = request.Price
                };

                await _unitOfWork.Products.AddAsync(product);
                await _unitOfWork.CompleteAsync();

                return product;
            }
            catch (Exception ex)
            {

                throw new ApplicationException("An error occurred while adding the product.", ex);
            }

        }
    }
}
