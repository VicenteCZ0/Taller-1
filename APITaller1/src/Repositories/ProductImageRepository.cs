using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using APITaller1.src.data;
using APITaller1.src.interfaces;
using APITaller1.src.models;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace APITaller1.src.Repositories
{
    public class ProductImageRepository : IProductImageRepository
    {
        private readonly StoreContext _context;
        private readonly ILogger<ProductImage> _logger;

        public ProductImageRepository(StoreContext context, ILogger<ProductImage> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task AddProductImageAsync(ProductImage image)
        {
            await _context.ProductImages.AddAsync(image);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteProductImageAsync(ProductImage image)
        {
            _context.ProductImages.Remove(image);
            await _context.SaveChangesAsync();
        }

        public async Task<ProductImage> GetProductImageByIdAsync(int id)
        {
            return await _context.ProductImages
                .Include(img => img.Product)
                .FirstOrDefaultAsync(img => img.ImageID == id)
                ?? throw new Exception("Product image not found");
        }

        public async Task<IEnumerable<ProductImage>> GetImagesByProductIdAsync(int productId)
        {
            return await _context.ProductImages
                .Where(img => img.ProductID == productId)
                .Include(img => img.Product)
                .ToListAsync();
        }

        public async Task<IEnumerable<ProductImage>> GetAllProductImagesAsync()
        {
            return await _context.ProductImages
                .Include(img => img.Product)
                .ToListAsync();
        }

        public async Task UpdateProductImageAsync(ProductImage image)
        {
            var existingImage = await _context.ProductImages.FindAsync(image.ImageID)
                ?? throw new Exception("Product image not found");

            existingImage.Url_Image = image.Url_Image;
            existingImage.ProductID = image.ProductID;

            _context.ProductImages.Update(existingImage);
            await _context.SaveChangesAsync();
        }
    }
}
