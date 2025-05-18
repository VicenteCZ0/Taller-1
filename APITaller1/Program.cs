using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Text;

using APITaller1.src.data;
using APITaller1.src.interfaces;
using APITaller1.src.Repositories;
using APITaller1.src.Services;
using APITaller1.src.models;

var builder = WebApplication.CreateBuilder(args);

// Establecer URLs de escucha
builder.WebHost.UseUrls("https://localhost:7283", "http://localhost:5000");

// Configurar Serilog
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .WriteTo.Console()
    .Enrich.FromLogContext()
    .Enrich.WithThreadId()
    .Enrich.WithMachineName()
    .CreateLogger();

builder.Host.UseSerilog((context, services, configuration) =>
{
    configuration
        .ReadFrom.Configuration(context.Configuration)
        .WriteTo.Console()
        .Enrich.FromLogContext()
        .Enrich.WithThreadId()
        .Enrich.WithMachineName();
});

try
{
    Log.Information("Starting server...");

    // Agregar servicios básicos
    builder.Services.AddControllers();

    // DbContext
    builder.Services.AddDbContext<StoreContext>(options =>
        options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

    // Identity
    builder.Services.AddIdentity<User, IdentityRole<int>>()
        .AddEntityFrameworkStores<StoreContext>()
        .AddDefaultTokenProviders();

    builder.Services.AddScoped<PasswordHasher<User>>();

    // Repositorios
    builder.Services.AddScoped<IUserRepository, UserRepository>();
    builder.Services.AddScoped<IProductRepository, ProductRepository>();
    builder.Services.AddScoped<IProductImageRepository, ProductImageRepository>();
    builder.Services.AddScoped<IStatusRepository, StatusRepository>();
    builder.Services.AddScoped<IShoppingCartRepository, ShoppingCartRepository>();
    builder.Services.AddScoped<ICartItemRepository, CartItemRepository>();

    // Servicios adicionales
    builder.Services.AddScoped<CartItemService>();
    builder.Services.AddScoped<TokenService>();

    // Unit of Work
    builder.Services.AddScoped<UnitOfWork>();

    // Autenticación JWT
    builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            var key = Encoding.UTF8.GetBytes(builder.Configuration["JWT:SignInKey"]);
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = builder.Configuration["JWT:Issuer"],
                ValidAudience = builder.Configuration["JWT:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(key)
            };
        });

    builder.Services.AddAuthorization();

    var app = builder.Build();

    // Inicialización de la base de datos
    DbInitializer.InitDb(app);

    app.UseAuthentication();
    app.UseAuthorization();

    app.MapControllers();

    // Log de URLs de escucha
    var lifetime = app.Services.GetRequiredService<IHostApplicationLifetime>();
    lifetime.ApplicationStarted.Register(() =>
    {
        var serverAddresses = app.Urls;
        foreach (var address in serverAddresses)
        {
            Console.WriteLine($"Server is listening on: {address}");
        }
    });

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Server terminated unexpectedly");
    Console.WriteLine(" ERROR DETALLES: " + ex);
}
finally
{
    Log.CloseAndFlush();
}
