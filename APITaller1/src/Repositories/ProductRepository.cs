using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using APITaller1.src.data;
using APITaller1.src.interfaces;
using APITaller1.src.models;

using Microsoft.EntityFrameworkCore;

namespace APITaller1.src.Repositories;

public class ProductRepository(StoreContext store, ILogger<Product> logger): IProductRepository
{
    private readonly StoreContext _context = store;
    private readonly ILogger<Product> _logger = logger;
    
    public async Task AddProductAsync(Product product)
    {
        await _context.Products.AddAsync(product);
    }

    public void DeleteProductAsync(Product product)
    {
        _context.Products.Remove(product);
    }

    public async Task<Product> GetProductByIdAsync(int id)
    {
        return await _context.Products.FindAsync(id) ?? throw new Exception("Product not found");
    }

    public async Task<IEnumerable<Product>> GetProductsAsync()
    {
        return await _context.Products.ToListAsync() ?? throw new Exception("No products found");
    }

    public async Task UpdateProductAsync(Product product)
    {
        var existingProduct = await _context.Products.FindAsync(product.ProductID) ?? throw new Exception("Product not found");
        existingProduct.Name = product.Name;
        existingProduct.Description = product.Description;
        existingProduct.Price = product.Price;
        existingProduct.Stock = product.Stock;
        existingProduct.Category = product.Category;
        existingProduct.Urls = product.Urls;
        existingProduct.Brand = product.Brand;
        existingProduct.StatusID = product.StatusID;
        _context.Products.Update(existingProduct);
    }
}