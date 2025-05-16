using Microsoft.EntityFrameworkCore;
using APITaller1.src.data;
using APITaller1.src.Repositories;
using Serilog;
using APITaller1.src.interfaces; 

var builder = WebApplication.CreateBuilder(args);

// Configura Serilog leyendo del archivo appsettings.json (si tienes uno)
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration) // <-- conecta config.json
    .Enrich.FromLogContext()
    .CreateLogger();

builder.Host.UseSerilog(); // <-- conecta Serilog con ASP.NET Core
try
{
    Log.Information("starting server.");
    //var builder = WebApplication.CreateBuilder(args);
    builder.Services.AddControllers();
    builder.Services.AddDbContext<StoreContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));
    builder.Services.AddScoped<IUserRepository, UserRepository>();
    builder.Services.AddScoped<IProductRepository, ProductRepository>();
    builder.Services.AddScoped<IRoleRepository, RoleRepository>();
    builder.Services.AddScoped<IProductImageRepository, ProductImageRepository>();
    builder.Services.AddScoped<IStatusRepository, StatusRepository>();
    builder.Services.AddScoped<UnitOfWork>();
    builder.Host.UseSerilog((context, services, configuration) =>
    {
        configuration
            .ReadFrom.Configuration(context.Configuration)
            .Enrich.FromLogContext()
            .Enrich.WithThreadId()
            .Enrich.WithMachineName();

    });

    var app = builder.Build();
    DbInitializer.InitDb(app);
    app.MapControllers();
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
