namespace OrdersApi.Models
{
    public class Customer
    {
        public Customer()
        {
            _orders = new List<Order>();
        }
        public Customer(string firstName, string lastName, string address, string postalCode)
        {
            FirstName = firstName;
            LastName = lastName;
            Address = address;
            PostalCode = postalCode;

        }

        public int CustomerId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string PostalCode { get; set; }
        private readonly List<Order> _orders;
        public IReadOnlyCollection<Order> Orders => _orders.AsReadOnly();
        public void AddOrder(Order order)
        {
            _orders.Add(order);
        }
    }
}
