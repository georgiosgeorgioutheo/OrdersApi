using Microsoft.AspNetCore.Mvc.Testing;
using Moq;
using MediatR;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using OrdersApi.Commands;
using OrdersApi.Models;
using OrdersApi.Queries;
using System.Collections.Generic;
using Castle.Core.Resource;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Microsoft.Extensions.DependencyInjection;
using static OrdersApi.Commands.CreateOrderCommand;
using Newtonsoft.Json;
using Azure.Core;

namespace OrdersApi.Tests
{
    [TestFixture]
    public class OrdersApiTests
    {
        private HttpClient _client;
        private Mock<IMediator> _mediatorMock;
        private WebApplicationFactory<Program> _factory;

        [SetUp]
        public void Setup()
        {
            _mediatorMock = new Mock<IMediator>();

            _factory = new WebApplicationFactory<Program>()
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureServices(services =>
                    {
                        services.AddScoped(_ => _mediatorMock.Object);
                    });
                });

            _client = _factory.CreateClient();
        }

        [TearDown]
        public void TearDown()
        {
            _client?.Dispose();
            _factory?.Dispose();
        }
        [Test]
        public async Task CreateCustomer_ReturnsOk()
        {
            // Arrange
            var command = new CreateCustomerCommand
            {
                FirstName = "John",
                LastName = "Doe",
                Address = "123 Main St",
                PostalCode = "12345"
            };

            var customer = new Customer
            {
                CustomerId = 1,
                FirstName = "John",
                LastName = "Doe",
                Address = "123 Main St",
                PostalCode = "12345"
            };

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<CreateCustomerCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(customer);

            var jsonContent = new StringContent(System.Text.Json.JsonSerializer.Serialize(command), Encoding.UTF8, "application/json");
            var tss =await jsonContent.ReadAsStringAsync();
            // Act
            var response = await _client.PostAsync("/api/customers", jsonContent);

            // Assert
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            var responseCustomer = System.Text.Json.JsonSerializer.Deserialize<Customer>(responseString, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            Assert.AreEqual(customer.CustomerId, responseCustomer.CustomerId);
            Assert.AreEqual(customer.FirstName, responseCustomer.FirstName);
            Assert.AreEqual(customer.LastName, responseCustomer.LastName);
            Assert.AreEqual(customer.Address, responseCustomer.Address);
            Assert.AreEqual(customer.PostalCode, responseCustomer.PostalCode);
        }

        [Test]
        public async Task DeleteCustomer_ReturnsNoContent()
        {
            // Arrange
            var customerId = 1;

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<DeleteCustomerCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Unit.Value);

            // Act
            var response = await _client.DeleteAsync($"/api/customers/{customerId}");

            // Assert
            Assert.AreEqual(System.Net.HttpStatusCode.NoContent, response.StatusCode);
        }

        [Test]
        public async Task GetAllCustomers_ReturnsOk()
        {
            // Arrange
            var customers = new List<Customer>
            {
                new Customer { CustomerId = 1, FirstName = "John", LastName = "Doe", Address = "123 Main St", PostalCode = "12345" },
                new Customer { CustomerId = 2, FirstName = "Jane", LastName = "Doe", Address = "456 Elm St", PostalCode = "67890" }
            };

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<GetAllCustomersQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(customers);

            // Act
            var response = await _client.GetAsync("/api/customers");

            // Assert
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            var responseCustomers = System.Text.Json.JsonSerializer.Deserialize<List<Customer>>(responseString, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            Assert.AreEqual(customers.Count, responseCustomers.Count);
        }

        [Test]
        public async Task GetCustomerById_ReturnsOk()
        {
            // Arrange
            var customerId = 1;
            var customer = new Customer { CustomerId = customerId, FirstName = "John", LastName = "Doe", Address = "123 Main St", PostalCode = "12345" };

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<GetCustomerByIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(customer);

            // Act
            var response = await _client.GetAsync($"/api/customers/{customerId}");

            // Assert
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            var responseCustomer = System.Text.Json.JsonSerializer.Deserialize<Customer>(responseString, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            Assert.AreEqual(customer.CustomerId, responseCustomer.CustomerId);
            Assert.AreEqual(customer.FirstName, responseCustomer.FirstName);
            Assert.AreEqual(customer.LastName, responseCustomer.LastName);
            Assert.AreEqual(customer.Address, responseCustomer.Address);
            Assert.AreEqual(customer.PostalCode, responseCustomer.PostalCode);
        }
        [Test]
        public async Task CreateOrder_ReturnsOk()
        {
            // Arrange
            var command = new CreateOrderCommand
            {
                CustomerId = 1,
                OrderDate = DateTime.UtcNow,
                Items = new List<OrderItemDto>()
                {
                    new OrderItemDto { ProductId = 1, Quantity = 2, Price = 10.0m },
                    new OrderItemDto { ProductId = 2, Quantity = 1, Price = 20.0m }
                }
            };

            var order = new Order
            {
                CustomerId = command.CustomerId,
                OrderDate = command.OrderDate,
                Items = new List<OrderItem>()
            };
           

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<CreateOrderCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(order);
            
            var jsonContent = new StringContent(JsonConvert.SerializeObject(command), Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync("/api/orders", jsonContent);
           
            // Assert
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
          
            var responseOrder = JsonConvert.DeserializeObject<Order>(responseString);
           
            Assert.AreEqual(order.CustomerId, responseOrder.CustomerId);
            Assert.AreEqual(order.OrderDate, responseOrder.OrderDate);
            Assert.AreEqual(order.Items.Count, responseOrder.Items.Count);
            Assert.AreEqual(order.TotalPrice, responseOrder.TotalPrice);
        }

        [Test]
        public async Task DeleteOrder_ReturnsNoContent()
        {
            // Arrange
            var orderId = 1;

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<DeleteOrderCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Unit.Value);

            // Act
            var response = await _client.DeleteAsync($"/api/orders/{orderId}");

            // Assert
            Assert.AreEqual(System.Net.HttpStatusCode.NoContent, response.StatusCode);
        }

        [Test]
        public async Task GetAllOrders_ReturnsOk()
        {
            // Arrange
            var order1 = new Order  
            {
                CustomerId = 1,
                OrderDate = DateTime.UtcNow,
                Items = new List<OrderItem>()
            };
            var order2 = new Order
            {
                CustomerId = 2,
                OrderDate = DateTime.UtcNow,
                Items = new List<OrderItem>()
            };
            var orders = new List<Order>
            {
               order1,
               order2
            };

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<GetAllOrdersQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(orders);

            // Act
            var response = await _client.GetAsync("/api/orders");

            // Assert
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            var responseOrders = System.Text.Json.JsonSerializer.Deserialize<List<Order>>(responseString, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            Assert.AreEqual(orders.Count, responseOrders.Count);
        }

        [Test]
        public async Task GetOrderById_ReturnsOk()
        {
            // Arrange
            var orderId = 1;
             var order = new Order
            {
                CustomerId = 1,
                OrderDate = DateTime.UtcNow,
                Items = new List<OrderItem>()
            };


            _mediatorMock
                .Setup(m => m.Send(It.IsAny<GetOrderByIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(order);

            // Act
            var response = await _client.GetAsync($"/api/orders/{orderId}");

            // Assert
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            var responseOrder = System.Text.Json.JsonSerializer.Deserialize<Order>(responseString, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            Assert.AreEqual(order.OrderDate, responseOrder.OrderDate);
        }
    }
}

