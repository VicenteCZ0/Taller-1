using Microsoft.EntityFrameworkCore;
using APITaller1.src.data;
using APITaller1.src.Repositories;
using Serilog;
using APITaller1.src.interfaces;
using APITaller1.src.services;
using APITaller1.src.models;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Identity;


var builder = WebApplication.CreateBuilder(args);

builder.WebHost.UseUrls("https://localhost:7283", "http://localhost:5000");

// Configura Serilog leyendo del archivo appsettings.json (si tienes uno)
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .WriteTo.Console() // <-- Agrega esto
    .Enrich.FromLogContext()    
    .CreateLogger();

builder.Host.UseSerilog((context, services, configuration) =>
{
    configuration
        .ReadFrom.Configuration(context.Configuration)
        .WriteTo.Console() // <-- Asegúrate que esto esté aquí también
        .Enrich.FromLogContext()
        .Enrich.WithThreadId()
        .Enrich.WithMachineName();
});// <-- conecta Serilog con ASP.NET Core
try
{
    Log.Information("starting server.");
    //var builder = WebApplication.CreateBuilder(args);
    builder.Services.AddControllers();
    builder.Services.AddDbContext<StoreContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));
    builder.Services.AddIdentity<User, IdentityRole<int>>()
    .AddEntityFrameworkStores<StoreContext>()
    .AddDefaultTokenProviders();
    builder.Services.AddScoped<IUserRepository, UserRepository>();
    builder.Services.AddScoped<IProductRepository, ProductRepository>();
    builder.Services.AddScoped<IProductImageRepository, ProductImageRepository>();
    builder.Services.AddScoped<IStatusRepository, StatusRepository>();
    builder.Services.AddScoped<UnitOfWork>();
    builder.Services.AddScoped<TokenService>();
    builder.Services.AddScoped<PasswordHasher<User>>();
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
    DbInitializer.InitDb(app);
    app.UseAuthentication();
    app.UseAuthorization();
    app.MapControllers();

    // Imprimir las URLs correctamente DESPUÉS de iniciar el servidor
    var lifetime = app.Services.GetRequiredService<IHostApplicationLifetime>();
    lifetime.ApplicationStarted.Register(() =>
    {
        var serverAddresses = app.Urls;
        foreach (var address in serverAddresses)
        {
            Console.WriteLine($"✅ Server is listening on: {address}");
        }
    });

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "server terminated unexpectedly");
    Console.WriteLine("ERROR DETALLES: " + ex.ToString());
}
finally
{
    Log.CloseAndFlush();
}
