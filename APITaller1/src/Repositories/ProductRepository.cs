using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using APITaller1.src.data;
using APITaller1.src.interfaces;
using APITaller1.src.models;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace APITaller1.src.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly StoreContext _context;
    private readonly ILogger<Product> _logger;

    public ProductRepository(StoreContext store, ILogger<Product> logger)
    {
        _context = store;
        _logger = logger;
    }

    public async Task AddProductAsync(Product product)
    {
        await _context.Products.AddAsync(product);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteProductAsync(Product product)
    {
        _context.Products.Remove(product);
        await _context.SaveChangesAsync();
    }

    public async Task<Product> GetProductByIdAsync(int id)
    {
        return await _context.Products
            .Include(p => p.Status)
            .Include(p => p.ProductImages)
            .FirstOrDefaultAsync(p => p.ProductID == id)
            ?? throw new Exception("Product not found");
    }

    public async Task<IEnumerable<Product>> GetProductsAsync()
    {
        return await _context.Products
            .Include(p => p.Status)
            .Include(p => p.ProductImages)
            .ToListAsync();
    }

    public async Task UpdateProductAsync(Product product)
    {
        var existingProduct = await _context.Products.FindAsync(product.ProductID)
            ?? throw new Exception("Product not found");

        existingProduct.Name = product.Name;
        existingProduct.Description = product.Description;
        existingProduct.Price = product.Price;
        existingProduct.Stock = product.Stock;
        existingProduct.Category = product.Category;
        existingProduct.Urls = product.Urls;
        existingProduct.Brand = product.Brand;
        existingProduct.StatusID = product.StatusID;

        _context.Products.Update(existingProduct);
        await _context.SaveChangesAsync();
    }

    // Métodos adicionales opcionales

    public async Task<IEnumerable<Product>> GetProductsByCategoryAsync(string category)
    {
        return await _context.Products
            .Where(p => p.Category.ToLower() == category.ToLower())
            .Include(p => p.Status)
            .Include(p => p.ProductImages)
            .ToListAsync();
    }

    public async Task<IEnumerable<Product>> SearchProductsByNameAsync(string keyword)
    {
        return await _context.Products
            .Where(p => p.Name.ToLower().Contains(keyword.ToLower()))
            .Include(p => p.Status)
            .Include(p => p.ProductImages)
            .ToListAsync();
    }
}
