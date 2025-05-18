using System;
using System.Collections.Generic;
using System.Linq;
using Bogus;
using Microsoft.EntityFrameworkCore;
using APITaller1.src.models;

namespace APITaller1.src.data
{
    public class DbInitializer
    {
        public static void InitDb(WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<StoreContext>();
            SeedData(context);
        }

        private static void SeedData(StoreContext context)
        {
            context.Database.Migrate();

            if (context.Products.Any() || context.Users.Any()) return;

            var faker = new Faker("es");

            // Crear roles
            if (!context.Roles.Any())
            {
                context.Roles.AddRange(new List<Role>
                {
                    new Role { RolName = "Admin" },
                    new Role { RolName = "User" }
                });
                context.SaveChanges();
            }

            // Crear administrador
            if (!context.Users.Any(u => u.Email == "ignacio.mancilla@gmail.com"))
            {
                var adminRole = context.Roles.First(r => r.RolName == "Admin");

                var admin = new User
                {
                    FirstName = "Ignacio",
                    LastName = "Mancilla",
                    Email = "ignacio.mancilla@gmail.com",
                    Password = "Pa$$word2025",
                    RoleID = adminRole.RoleID,
                    AccountStatus = true,
                    Telephone = "+56912345678",
                    DateOfBirth = new DateTime(1990, 1, 1),
                    LastLogin = DateTime.Now
                };

                context.Users.Add(admin);
                context.SaveChanges();

                context.ShippingAddress.Add(new ShippingAddress
                {
                    UserId = admin.UserID,
                    Street = faker.Address.StreetName(),
                    Number = faker.Address.BuildingNumber(),
                    Commune = faker.Address.City(),
                    Region = faker.Address.State(),
                    PostalCode = faker.Address.ZipCode()
                });
                context.SaveChanges();
            }

            // Estados de productos
            var statusNames = new[] { "Active", "Inactive", "OutOfStock" };
            foreach (var status in statusNames)
            {
                if (!context.Status.Any(s => s.StatusName == status))
                {
                    context.Status.Add(new Status { StatusName = status });
                }
            }
            context.SaveChanges();

            var activeStatusId = context.Status.First(s => s.StatusName == "Active").StatusID;

            // Productos
            var urls = new[]
            {
                "https://res.cloudinary.com/demo/image/upload/sample1.jpg",
                "https://res.cloudinary.com/demo/image/upload/sample2.jpg",
                "https://res.cloudinary.com/demo/image/upload/sample3.jpg"
            };

            var products = new Faker<Product>("es")
                .RuleFor(p => p.Name, f => f.Commerce.ProductName())
                .RuleFor(p => p.Category, f => f.Commerce.Department())
                .RuleFor(p => p.Description, f => f.Commerce.ProductDescription())
                .RuleFor(p => p.Price, f => f.Random.Int(5000, 50000))
                .RuleFor(p => p.Brand, f => f.Company.CompanyName())
                .RuleFor(p => p.Stock, f => f.Random.Int(10, 200))
                .RuleFor(p => p.StatusID, _ => activeStatusId)
                .RuleFor(p => p.CreatedAt, _ => DateTime.Now)
                .Generate(10);

            context.Products.AddRange(products);
            context.SaveChanges();

            foreach (var product in products)
            {
                context.ProductImages.Add(new ProductImage
                {
                    ProductID = product.ProductID,
                    Url_Image = faker.PickRandom(urls)
                });
            }

            // Usuarios de prueba
            if (context.Users.Count() <= 1)
            {
                var userRole = context.Roles.First(r => r.RolName == "User");

                var users = new Faker<User>("es")
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

                foreach (var user in users)
                {
                    context.ShippingAddress.Add(new ShippingAddress
                    {
                        UserId = user.UserID,
                        Street = faker.Address.StreetName(),
                        Number = faker.Address.BuildingNumber(),
                        Commune = faker.Address.City(),
                        Region = faker.Address.State(),
                        PostalCode = faker.Address.ZipCode()
                    });
                }
            }

            context.SaveChanges();
        }
    }
}
