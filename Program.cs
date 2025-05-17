using Hoved_Opgave_Datamatiker.DBContext;
using Hoved_Opgave_Datamatiker.Interfaces;
using Hoved_Opgave_Datamatiker.Pay;
using Hoved_Opgave_Datamatiker.Repository;
using Hoved_Opgave_Datamatiker.Repository.DBRepos;
using Hoved_Opgave_Datamatiker.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Stripe;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.Configure<StripeSettings>(builder.Configuration.GetSection("Stripe"));

// Register StripeClient as a singleton
builder.Services.AddSingleton<IStripeClient>(sp =>
{
    var stripeSettings = sp.GetRequiredService<IOptions<StripeSettings>>().Value;
    return new StripeClient(stripeSettings.SecretKey);
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//builder.Services.AddSingleton<ICustomerRepo, CustomerRepo>();
builder.Services.AddScoped<ICustomerRepo, CustomerDBrepo>();
builder.Services.AddScoped<ICustomerService, Hoved_Opgave_Datamatiker.Services.CustomerService>();
builder.Services.AddScoped<PaymentService>();

builder.Services.AddScoped<IDeliveryDateService, DeliveryDateService>();
builder.Services.AddSingleton<IDeliveryDateRepo, DeliveryDateRepo>();
//builder.Services.AddSingleton<IProductRepo, ProductRepo>();
builder.Services.AddScoped<IProductRepo, ProductDBrepo>();
builder.Services.AddSingleton<IBasketService, BasketService>();
builder.Services.AddScoped<IOrderService, OrderService>();




//builder.Services.AddSingleton<IOrderRepo, OrderRepo>();
builder.Services.AddScoped<IOrderRepo, OrderDBrepo>();
builder.Services.AddDbContext<AppDBContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
