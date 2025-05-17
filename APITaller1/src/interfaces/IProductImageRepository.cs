using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using APITaller1.src.models;

namespace APITaller1.src.interfaces
{
    public interface IProductImageRepository
    {
        Task AddProductImageAsync(ProductImage image);
        Task DeleteProductImageAsync(ProductImage image);
        Task<ProductImage> GetProductImageByIdAsync(int id);
        Task<IEnumerable<ProductImage>> GetImagesByProductIdAsync(int productId);
        Task<IEnumerable<ProductImage>> GetAllProductImagesAsync();
        Task UpdateProductImageAsync(ProductImage image);
    }
}
