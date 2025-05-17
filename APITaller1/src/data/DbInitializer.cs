using System;
using System.Collections.Generic;
using System.Linq;
using Bogus;
using Microsoft.EntityFrameworkCore;
using APITaller1.src.models;

namespace APITaller1.src.data;

public class DbInitializer
{
    public static void InitDb(WebApplication app)
    {
        using var scope = app.Services.CreateScope();

        var context = scope.ServiceProvider.GetRequiredService<StoreContext>()
            ?? throw new InvalidOperationException("Could not get StoreContext");

        SeedData(context);
    }

    private static void SeedData(StoreContext context)
    {
        context.Database.Migrate();

        if (context.Products.Any() || context.Users.Any()) return;

        var faker = new Faker("es");

        // Crear los roles
        if (!context.Roles.Any())
        {
            var roles = new List<Role>
            {
                new Role { RolName = "Admin" },
                new Role { RolName = "User" }
            };

            context.Roles.AddRange(roles);
            context.SaveChanges();
        }

        // Crear el usuario administrador si no existe
        var adminExists = context.Users.Any(u => u.Email == "ignacio.mancilla@gmail.com");
        if (!adminExists)
        {
            var adminRole = context.Roles.FirstOrDefault(r => r.RolName == "Admin");
            if (adminRole == null)
            {
                adminRole = new Role { RolName = "Admin" };
                context.Roles.Add(adminRole);
                context.SaveChanges();
            }

            var admin = new User
            {
                FirstName = "Ignacio",
                LastName = "Mancilla",
                Email = "ignacio.mancilla@gmail.com",
                Password = "Pa$$word2025",
                RoleID = adminRole.RoleID,
                Role = adminRole,
                AccountStatus = true,
                Telephone = "+56912345678",
                DateOfBirth = new DateTime(1990, 1, 1),
                LastLogin = DateTime.Now
            };

            context.Users.Add(admin);
            context.SaveChanges();

            // Agregar dirección aleatoria para el admin
            var adminAddress = new ShippingAddress
            {
                UserId = admin.UserID,
                Street = faker.Address.StreetName(),
                Number = faker.Address.BuildingNumber(),
                Commune = faker.Address.City(),
                Region = faker.Address.State(),
                PostalCode = faker.Address.ZipCode()
            };
            context.ShippingAddress.Add(adminAddress);
            context.SaveChanges();
        }

        // Crear estados de producto
        var statusNames = new[] { "Active", "Inactive", "OutOfStock" };
        foreach (var statusName in statusNames)
        {
            if (!context.Status.Any(s => s.StatusName == statusName))
            {
                context.Status.Add(new Status { StatusName = statusName });
            }
        }
        context.SaveChanges();

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
        context.SaveChanges();

        foreach (var product in products)
        {
            var image = new ProductImage
            {
                ProductID = product.ProductID,
                Url_Image = faker.PickRandom(urls)
            };
            context.ProductImages.Add(image);
        }

        // Generar usuarios de prueba si solo existe el admin
        if (context.Users.Count() <= 1)
        {
            var userRole = context.Roles.FirstOrDefault(r => r.RolName == "User");
            if (userRole == null)
            {
                userRole = new Role { RolName = "User" };
                context.Roles.Add(userRole);
                context.SaveChanges();
            }

            // Configurar Faker para generar usuarios
            var users = new Faker<User>()
                .RuleFor(u => u.FirstName, f => f.Person.FirstName)
                .RuleFor(u => u.LastName, f => f.Person.LastName)
                .RuleFor(u => u.Email, (f, u) => f.Internet.Email(u.FirstName, u.LastName))
                .RuleFor(u => u.Password, f => f.Internet.Password(10, false))
                .RuleFor(u => u.RoleID, _ => userRole.RoleID)
                .RuleFor(u => u.Telephone, f => f.Phone.PhoneNumber())
                .RuleFor(u => u.DateOfBirth, f => f.Date.Past(50, DateTime.Now.AddYears(-18)))
                .RuleFor(u => u.AccountStatus, true)
                .RuleFor(u => u.LastLogin, f => f.Date.Recent(90))
                .Generate(5);

            context.Users.AddRange(users);
            context.SaveChanges();

            // Generar exactamente una dirección de envío para cada usuario
            foreach (var user in users)
            {
                var address = new ShippingAddress
                {
                    UserId = user.UserID,  // Relación con el usuario correspondiente
                    Street = faker.Address.StreetName(),
                    Number = faker.Address.BuildingNumber(),
                    Commune = faker.Address.City(),
                    Region = faker.Address.State(),
                    PostalCode = faker.Address.ZipCode()
                };
                
                context.ShippingAddress.Add(address);
            }
            
        }

        context.SaveChanges();
    }
}