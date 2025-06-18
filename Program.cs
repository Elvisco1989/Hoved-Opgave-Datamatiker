using Hoved_Opgave_Datamatiker.APPDBContext;
using Hoved_Opgave_Datamatiker.DBContext;
using Hoved_Opgave_Datamatiker.Interfaces;
using Hoved_Opgave_Datamatiker.Pay;
using Hoved_Opgave_Datamatiker.Repository;
using Hoved_Opgave_Datamatiker.Repository.DBRepos;
using Hoved_Opgave_Datamatiker.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Stripe;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
//builder.Services.Configure<StripeSettings>(builder.Configuration.GetSection("Stripe"));

//// Register StripeClient as a singleton
//builder.Services.AddSingleton<IStripeClient>(sp =>
//{
//    var stripeSettings = sp.GetRequiredService<IOptions<StripeSettings>>().Value;
//    return new StripeClient(stripeSettings.SecretKey);
//});


builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
 
});




// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//builder.Services.AddSingleton<ICustomerRepo, CustomerRepo>();
builder.Services.AddScoped<ICustomerRepo, CustomerDBrepo>();
builder.Services.AddScoped<ICustomerService, Hoved_Opgave_Datamatiker.Services.CustomerService>();
//builder.Services.AddScoped<PaymentService>();

builder.Services.AddScoped<IDeliveryDateService, DeliveryDateService>();
builder.Services.AddSingleton<IDeliveryDateRepo, DeliveryDateRepo>();
//builder.Services.AddSingleton<IProductRepo, ProductRepo>();
builder.Services.AddScoped<IProductRepo, ProductDBrepo>();
builder.Services.AddSingleton<IBasketService, BasketService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IProductService, Hoved_Opgave_Datamatiker.Services.ProductService>();




//builder.Services.AddSingleton<IOrderRepo, OrderRepo>();
builder.Services.AddScoped<IOrderRepo, OrderDBrepo>();
builder.Services.AddDbContext<AppDBContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddDbContext<LoginDBContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("Login")));
builder.Services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<LoginDBContext>()

.AddEntityFrameworkStores<LoginDBContext>()
.AddDefaultTokenProviders();
builder.Services.AddAuthorization();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseStaticFiles(); // This enables wwwroot serving


app.UseCors();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();
app.MapGet("/", () => Results.Redirect("/swagger"));


app.Run();
