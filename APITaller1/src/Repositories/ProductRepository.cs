using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using APITaller1.src.data;
using APITaller1.src.interfaces;
using APITaller1.src.models;
using APITaller1.src.Helpers;

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
        //existingProduct.Urls = product.Urls;
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


    public async Task<Product?> GetByIdWithImagesAsync(int id)
    {
        return await _context.Products
            .Include(p => p.Status)        // Carga el Status
            .Include(p => p.ProductImages) // Carga las imágenes
            .FirstOrDefaultAsync(p => p.ProductID == id);
    }

    /*
        public async Task<bool> HasOrdersAsync(int productId)
        {
            return await _context.OrderItems.AnyAsync(o => o.ProductID == productId);
        }

    */
    public async Task<Product?> GetByIdAsync(int id)
    {
        return await _context.Products.FindAsync(id);
    }


    public async Task<PagedList<Product>> GetCatalogAsync(ProductQueryParams queryParams)
    {
        var query = _context.Products
            .Include(p => p.ProductImages)
            .Include(p => p.Status)
            .AsQueryable();

        if (!string.IsNullOrEmpty(queryParams.Category))
            query = query.Where(p => p.Category.ToLower() == queryParams.Category.ToLower());

        if (queryParams.MinPrice.HasValue)
            query = query.Where(p => p.Price >= queryParams.MinPrice.Value);

        if (queryParams.MaxPrice.HasValue)
            query = query.Where(p => p.Price <= queryParams.MaxPrice.Value);

        if (!string.IsNullOrEmpty(queryParams.Brand))
            query = query.Where(p => p.Brand.ToLower() == queryParams.Brand.ToLower());

        if (!string.IsNullOrEmpty(queryParams.Search))
            query = query.Where(p =>
                p.Name.ToLower().Contains(queryParams.Search.ToLower()) ||
                p.Description.ToLower().Contains(queryParams.Search.ToLower()));

        // Ordenamiento
        query = queryParams.Sort switch
        {
            "priceAsc" => query.OrderBy(p => p.Price),
            "priceDesc" => query.OrderByDescending(p => p.Price),
            "az" => query.OrderBy(p => p.Name),
            "za" => query.OrderByDescending(p => p.Name),
            _ => query.OrderBy(p => p.ProductID)
        };

        return await PagedList<Product>.CreateAsync(query, queryParams.PageNumber, queryParams.PageSize);
    }

    public async Task<PagedList<Product>> GetAdminListAsync(AdminProductQueryParams queryParams)
    {
        var query = _context.Products
            .Include(p => p.ProductImages)
            .Include(p => p.Status)
            .AsQueryable();

        // Puedes aplicar los mismos filtros que en el catálogo

        return await PagedList<Product>.CreateAsync(query, queryParams.PageNumber, queryParams.PageSize);
    }
}
