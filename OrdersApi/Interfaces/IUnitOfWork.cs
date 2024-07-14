namespace OrdersApi.Interfaces
{
    public interface IUnitOfWork
    {
        ICustomerRepository Customers { get; }
        IOrderRepository Orders { get; }

        IProductRepository Products { get; }
        Task<int> CompleteAsync();
    }
}
