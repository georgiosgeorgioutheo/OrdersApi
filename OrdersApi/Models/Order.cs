using Moq;

namespace OrdersApi.Models
{
    public class Order
    {
      
            public int OrderId { get; set; }
            public int CustomerId { get; set; }
            public Customer Customer { get; set; }
            public DateTime OrderDate { get; set; }
            public decimal TotalPrice { get; set; }
            public ICollection<OrderItem> Items { get; set; }
        
    }


}
