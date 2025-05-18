using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bogus;
using Microsoft.EntityFrameworkCore;
using APITaller1.src.models;
using Microsoft.AspNetCore.Identity;

namespace APITaller1.src.data;

public class DbInitializer
{
    public static void InitDb(WebApplication app)
    {
        using var scope = app.Services.CreateScope();

        var context = scope.ServiceProvider.GetRequiredService<StoreContext>();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole<int>>>();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();

        SeedData(context, userManager, roleManager).Wait();
    }
 
    private static async Task SeedData(
        StoreContext context,
        UserManager<User> userManager,
        RoleManager<IdentityRole<int>> roleManager)
    {
        context.Database.Migrate();
        
        // Crear los roles en la tabla legacy Role y en Identity
        var roleNames = new[] { "Admin", "User" };

        foreach (var roleName in roleNames)
        {
            if (!await roleManager.RoleExistsAsync(roleName))
            {
                await roleManager.CreateAsync(new IdentityRole<int>(roleName));
            }
        }
        
        // Crear usuario admin
        var adminEmail = "ignacio.mancilla@gmail.com";
        var admin = await userManager.FindByEmailAsync(adminEmail);
        if (admin == null)
        {
            admin = new User
            {
                FirstName = "Ignacio",
                LastName = "Mancilla",
                Email = adminEmail,
                UserName = adminEmail,
                Telephone = "+56912345678",
                DateOfBirth = new DateTime(1990, 1, 1),
                AccountStatus = true,
                LastLogin = DateTime.Now
            };
            var result = await userManager.CreateAsync(admin, "Pa$word2025");
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(admin, "Admin");
                context.ShippingAddress.Add(new ShippingAddress
                {
                    UserId = admin.Id,
                    Street = "Admin St",
                    Number = "123",
                    Commune = "Santiago",
                    Region = "RM",
                    PostalCode = "0000000"
                });
                await context.SaveChangesAsync();
            }
        }
        
        // Crear usuarios de prueba
        if (!context.Users.Any(u => u.Email != adminEmail))
        {
            var faker = new Faker("es");
            for (int i = 0; i < 5; i++)
            {
                var email = faker.Internet.Email();
                var user = new User
                {
                    FirstName = faker.Name.FirstName(),
                    LastName = faker.Name.LastName(),
                    Email = email,
                    UserName = email,
                    Telephone = faker.Phone.PhoneNumber(),
                    DateOfBirth = faker.Date.Past(30, DateTime.Now.AddYears(-18)),
                    AccountStatus = true,
                    LastLogin = DateTime.Now
                };
                var result = await userManager.CreateAsync(user, "User123!");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, "User");
                    context.ShippingAddress.Add(new ShippingAddress
                    {
                        UserId = user.Id,
                        Street = faker.Address.StreetName(),
                        Number = faker.Address.BuildingNumber(),
                        Commune = faker.Address.City(),
                        Region = faker.Address.State(),
                        PostalCode = faker.Address.ZipCode()
                    });
                }
            }
            await context.SaveChangesAsync();
        }
        
        
        // Verificar si ya existen productos
        if (context.Products.Any()) return;
        
        var faker2 = new Faker("es");
        
        // Crear estados de producto
        var statusNames = new[] { "Active", "Inactive", "OutOfStock" };
        foreach (var statusName in statusNames)
        {
            if (!context.Status.Any(s => s.StatusName == statusName))
            {
                context.Status.Add(new Status { StatusName = statusName });
            }
        }
        await context.SaveChangesAsync();
        
        // Obtener el ID del status "Active"
        var activeStatus = context.Status.FirstOrDefault(s => s.StatusName == "Active");
        
        if (activeStatus == null)
            throw new InvalidOperationException("No se pudo encontrar el estado 'Active' después de guardarlo.");
        
        var activeStatusId = activeStatus.StatusID;
        
        // Generar productos
        var urls = new[]
        {
            "https://res.cloudinary.com/demo/image/upload/sample1.jpg",
            "https://res.cloudinary.com/demo/image/upload/sample2.jpg",
            "https://res.cloudinary.com/demo/image/upload/sample3.jpg"
        };
        
        var productFaker = new Faker<Product>("es")
            .RuleFor(p => p.Name, f => f.Commerce.ProductName())
            .RuleFor(p => p.Category, f => f.Commerce.Department())
            .RuleFor(p => p.Description, f => f.Commerce.ProductDescription())
            .RuleFor(p => p.Price, f => f.Random.Int(5000, 50000))
            .RuleFor(p => p.Brand, f => f.Company.CompanyName())
            .RuleFor(p => p.Stock, f => f.Random.Int(10, 200))
            .RuleFor(p => p.StatusID, _ => activeStatusId)
            .RuleFor(p => p.CreatedAt, _ => DateTime.Now);
        
        var products = productFaker.Generate(10);
        context.Products.AddRange(products);
        await context.SaveChangesAsync();
        
        foreach (var product in products)
        {
            var image = new ProductImage
            {
                ProductID = product.ProductID,
                Url_Image = faker2.PickRandom(urls)
            };
            context.ProductImages.Add(image);
        }
        
        await context.SaveChangesAsync();
    }
}
