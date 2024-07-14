using MediatR;
using OrdersApi.Commands;
using OrdersApi.Interfaces;
using OrdersApi.Models;

namespace OrdersApi.Handlers
{
    public class UpdateOrderHandler : IRequestHandler<UpdateOrderCommand, Order>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateOrderHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }



        /// <summary>
        /// Handles Update Order Command
        /// </summary>
        /// <param name="request">The request</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>
        /// The updated order
        /// </returns>
        public async Task<Order> Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var order = await _unitOfWork.Orders.GetByIdAsync(request.OrderId);
                if (order == null)
                    throw new KeyNotFoundException($"Order with ID {request.OrderId} was not found.");

                var updatedItems = new List<OrderItem>();

                foreach (var itemDto in request.Items)
                {
                    var product = await _unitOfWork.Products.GetByIdAsync(itemDto.ProductId);
                    if (product == null)
                    {
                        // if product is null ad new one
                        product = new Product
                        {
                            Name = $"New Product {itemDto.ProductId}",
                            Price = itemDto.Price
                        };
                        await _unitOfWork.Products.AddAsync(product);
                        await _unitOfWork.CompleteAsync();

                    }
                    //add order item
                    var orderItem = new OrderItem
                    {
                        ProductId = product.ProductId,
                        Quantity = itemDto.Quantity,
                        Price = itemDto.Price,
                        Product = product
                    };
                    order.Items.Add(orderItem);
                }
                // callculate total price
                order.TotalPrice = order.Items.Sum(i => i.Price * i.Quantity);


                _unitOfWork.Orders.Update(order);
                await _unitOfWork.CompleteAsync();

                return order;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An unexpected error occurred while updating the order.", ex);
            }
        }
    }
}
