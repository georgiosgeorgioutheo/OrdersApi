using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using OrdersApi.Commands;
using OrdersApi.Data;
using OrdersApi.Interfaces;
using OrdersApi.Queries;
using OrdersApi.Repositories;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));

builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

//app.UseAuthorization();




app.MapPost("/api/customers", async (CreateCustomerCommand command, IMediator mediator) =>
{
    var customer = await mediator.Send(command);
    return Results.Ok(customer);
})
.WithName("CreateCustomer")
 .WithOpenApi();



app.MapDelete("/api/customers/{id}", async (int id, IMediator mediator) =>
{
    await mediator.Send(new DeleteCustomerCommand(id));
    return Results.NoContent();
})
.WithName("DeleteCustomer")
.WithOpenApi();

app.MapGet("/api/customers", async (IMediator mediator) =>
{
    var customers = await mediator.Send(new GetAllCustomersQuery());
    return Results.Ok(customers);
})
.WithName("GetAllCustomers")
.WithOpenApi();

app.MapGet("/api/customers/{customerId}", async (int customerId, IMediator mediator) =>
{
    var customer = await mediator.Send(new GetCustomerByIdQuery(customerId));
    if (customer == null)
    {
        return Results.NotFound();
    }
    return Results.Ok(customer);
})
.WithName("GetCustomerById")
.WithOpenApi();


app.MapPost("/api/orders", async ( CreateOrderCommand command, IMediator mediator) =>
{
   

    var order = await mediator.Send(command);
    return Results.Ok(order);
})
.WithName("CreateOrder").WithOpenApi();

app.MapPut("/api/customers/{id}", async (int id, UpdateCustomerCommand command, IMediator mediator) =>
{
    if (id != command.CustomerId)
    {
        return Results.BadRequest();
    }

    var customer = await mediator.Send(command);
    if (customer == null)
    {
        return Results.NotFound();
    }

    return Results.Ok(customer);
})
.WithName("UpdateCustomer").WithOpenApi();

app.MapPut("/api/orders/{id}", async (int id, UpdateOrderCommand command, IMediator mediator) =>
{
    if (id != command.OrderId)
    {
        return Results.BadRequest();
    }

    var order = await mediator.Send(command);
    if (order == null)
    {
        return Results.NotFound();
    }

    return Results.Ok(order);
})
.WithName("UpdateOrder").WithOpenApi();

app.MapDelete("/api/orders/{id}", async (int id, IMediator mediator) =>
{
    await mediator.Send(new DeleteOrderCommand(id));
    return Results.NoContent();
})
.WithName("DeleteOrder").WithOpenApi();

app.MapGet("/api/orders", async (IMediator mediator) =>
{
    var orders = await mediator.Send(new GetAllOrdersQuery());
    return Results.Ok(orders);
})
.WithName("GetAllOrders")
.WithOpenApi();

app.MapGet("/api/orders/date", async (IMediator mediator) =>
{
    var orders = await mediator.Send(new GetOrdersOrderedByDateQuery());
    return Results.Ok(orders);
})
.WithName("GetOrdersByDate")
.WithOpenApi();

app.MapGet("/api/orders/{orderId}", async (int orderId, IMediator mediator) =>
{
    var order = await mediator.Send(new GetOrderByIdQuery(orderId));
    if (order == null)
    {
        return Results.NotFound();
    }
    return Results.Ok(order);
})
.WithName("GetOrderById").WithOpenApi();
app.Run();
public partial class Program { }