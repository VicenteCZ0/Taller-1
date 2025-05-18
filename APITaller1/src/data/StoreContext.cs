using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using APITaller1.src.models;
using Microsoft.EntityFrameworkCore;  
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace APITaller1.src.data;

public class StoreContext : IdentityDbContext<User, IdentityRole<int>, int>
{
    public StoreContext(DbContextOptions<StoreContext> options) : base(options)
    {
    }

    public DbSet<Product> Products { get; set; }
    public DbSet<ShippingAddress> ShippingAddress { get; set; }
    public DbSet<Status> Status { get; set; }
    public DbSet<ProductImage> ProductImages { get; set; }
    public DbSet<ShoppingCart> ShoppingCarts { get; set; }
    public DbSet<CartItem> CartItems { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configurar relación User - ShippingAddres (uno a muchos)
        modelBuilder.Entity<ShippingAddress>(entity =>
        {
            entity.HasKey(sa => sa.AddressID);
            
            entity.HasOne(sa => sa.User)
                .WithOne(u => u.ShippingAddress) // 
                .HasForeignKey<ShippingAddress>(sa => sa.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Order>()
            .HasOne<User>()
            .WithMany()
            .HasForeignKey(o => o.UserId);

        modelBuilder.Entity<OrderItem>()
            .HasOne<Order>()
            .WithMany()
            .HasForeignKey(oi => oi.OrderID);
        
        modelBuilder.Entity<OrderItem>()
            .HasOne<Product>()
            .WithMany()
            .HasForeignKey(oi => oi.ProductID);

        // Configurar relación User - ShoppingCart (uno a muchos)
        modelBuilder.Entity<ShoppingCart>()
            .HasOne(sc => sc.User)
            .WithOne(u => u.ShoppingCart)
            .HasForeignKey<ShoppingCart>(sc => sc.UserId)
            .OnDelete(DeleteBehavior.Cascade);


        // Configurar relación ShoppingCart - CartItem (uno a muchos)
        modelBuilder.Entity<CartItem>()
            .HasOne(ci => ci.ShoppingCart)
            .WithMany(sc => sc.CartItems)
            .HasForeignKey(ci => ci.ShoppingCartID)
            .OnDelete(DeleteBehavior.Cascade);

        // Configurar relación Product - CartItem (uno a muchos)
        modelBuilder.Entity<CartItem>()
            .HasOne(ci => ci.Product)
            .WithMany() 
            .HasForeignKey(ci => ci.ProductID)
            .OnDelete(DeleteBehavior.Restrict);


        // Configurar relación Product - ProductImage (uno a muchos)
        modelBuilder.Entity<ProductImage>(entity =>
        {
            entity.HasKey(pi => pi.ImageID);  // Definir explícitamente la clave primaria
            
            entity.HasOne(pi => pi.Product)
                .WithMany(p => p.ProductImages)  // Asume que añadirás esta propiedad a Product
                .HasForeignKey(pi => pi.ProductID)  // Usar ProductID, no ImageID
                .OnDelete(DeleteBehavior.Cascade);
        });

        // Configurar relación Product - Status (uno a muchos)
        modelBuilder.Entity<Product>()
            .HasOne(p => p.Status)
            .WithMany()
            .HasForeignKey(p => p.StatusID)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

