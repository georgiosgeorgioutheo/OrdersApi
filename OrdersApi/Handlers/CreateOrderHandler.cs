using MediatR;
using OrdersApi.Commands;
using OrdersApi.Interfaces;
using OrdersApi.Models;


namespace OrdersApi.Handlers
{


    public class CreateOrderHandler : IRequestHandler<CreateOrderCommand, Order>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateOrderHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Handles a Create Order Command
        /// </summary>
        /// <param name="request">The request</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>
        /// The new order
        /// </returns>
        public async Task<Order> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            try { 
            var order = new Order
            {
                CustomerId = request.CustomerId,
                OrderDate = request.OrderDate,
                Items = new List<OrderItem>()
            };

            foreach (var item in request.Items)
            {
                // Get the product
                var product = await _unitOfWork.Products.GetByIdAsync(item.ProductId);
                if (product == null)
                {
                    // If the product does not exist, create a new one
                    product = new Product
                    {
                        Name = $"New Product {item.ProductId}",
                        Price = item.Price
                    };
                    await _unitOfWork.Products.AddAsync(product);
                    await _unitOfWork.CompleteAsync();
                }

                // Add the order item
                var orderItem = new OrderItem
                {
                    ProductId = product.ProductId,
                    Quantity = item.Quantity,
                    Price = item.Price,
                    Product = product
                };
                order.Items.Add(orderItem);
            }

            // Calculate the total price
            order.TotalPrice = order.Items.Sum(i => i.Price * i.Quantity);

            await _unitOfWork.Orders.AddAsync(order);
            await _unitOfWork.CompleteAsync();

            return order;
        }
             catch (Exception ex)
            {
            
                throw new ApplicationException("An error occurred while creating the order.", ex);
            }
        }

    }
}
