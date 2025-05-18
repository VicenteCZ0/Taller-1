using Microsoft.EntityFrameworkCore;
using APITaller1.src.data;
using Serilog;
using APITaller1.src.Repositories; 
using APITaller1.src.interfaces;
using APITaller1.src.Services;

var builder = WebApplication.CreateBuilder(args);

// Configurar Serilog
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .WriteTo.Console() // Para ver logs como "Now listening on..."
    .Enrich.FromLogContext()
    .CreateLogger();

builder.Host.UseSerilog();

// Servicios
builder.Services.AddControllers();
builder.Services.AddDbContext<StoreContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Repositorios
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IShoppingCartRepository, ShoppingCartRepository>();
builder.Services.AddScoped<ICartItemRepository, CartItemRepository>();

// Servicios
builder.Services.AddScoped<CartItemService>();

// Unit of Work
builder.Services.AddScoped<UnitOfWork>();

var app = builder.Build();

DbInitializer.InitDb(app);
app.MapControllers();
app.Run();
