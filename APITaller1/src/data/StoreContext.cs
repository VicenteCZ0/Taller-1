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

        modelBuilder.Entity<User>()
        .HasOne(u => u.ShippingAddress)
        .WithOne(a => a.User)
        .HasForeignKey<ShippingAddress>(a => a.UserId)
        .IsRequired(false); // Permite que un usuario no tenga dirección

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
            .HasOne(o => o.User)
            .WithMany(u => u.Orders)
            .HasForeignKey(o => o.UserId)
            .OnDelete(DeleteBehavior.Cascade); 

        modelBuilder.Entity<OrderItem>()
            .HasOne(oi => oi.Product)
            .WithMany()
            .HasForeignKey(oi => oi.ProductID)
            .OnDelete(DeleteBehavior.Restrict);

        
        modelBuilder.Entity<OrderItem>()
            .HasOne<Product>()
            .WithMany()
            .HasForeignKey(oi => oi.ProductID);

        modelBuilder.Entity<ShoppingCart>()
            .HasOne(sc => sc.User)
            .WithOne(u => u.ShoppingCart)
            .HasForeignKey<ShoppingCart>(sc => sc.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<CartItem>()
            .HasOne(ci => ci.ShoppingCart)
            .WithMany(sc => sc.CartItems)
            .HasForeignKey(ci => ci.ShoppingCartID)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<CartItem>()
            .HasOne(ci => ci.Product)
            .WithMany() 
            .HasForeignKey(ci => ci.ProductID)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<ProductImage>(entity =>
        {
            entity.HasKey(pi => pi.ImageID);  
            
            entity.HasOne(pi => pi.Product)
                .WithMany(p => p.ProductImages)  
                .HasForeignKey(pi => pi.ProductID)  
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Product>()
            .HasOne(p => p.Status)
            .WithMany()
            .HasForeignKey(p => p.StatusID)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

