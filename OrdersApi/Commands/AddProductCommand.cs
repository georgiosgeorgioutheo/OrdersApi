using MediatR;
using OrdersApi.Models;

namespace OrdersApi.Commands
{
  
    public class AddProductCommand : IRequest<Product>
    {
        public string Name { get; set; }
        public decimal Price { get; set; }

        public AddProductCommand(string name, decimal price)
        {
            Name = name;
            Price = price;
        }
    }
}
